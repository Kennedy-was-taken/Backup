using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Backup.Databases.MSSQL
{
    public class MssqlRepository
    {
        private readonly IConfiguration configuration;
        private readonly string backupPath;

        public MssqlRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.backupPath = setBackupPath();
        }

        private string? GetConnectionString()
        {
            return configuration.GetSection("Databases:SQL Server").Value;
        }

        private string setBackupPath()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "SELECT TOP 1 LEFT(physical_device_name, LEN(physical_device_name) - CHARINDEX('\\', REVERSE(physical_device_name))) AS BackupDirectory " +
                    "FROM msdb.dbo.backupmediafamily " +
                    "WHERE media_set_id = (SELECT TOP 1 media_set_id " +
                    "FROM msdb.dbo.backupset " +
                    "ORDER BY backup_start_date DESC);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            var path = dt.Rows[0][0].ToString();

            return path;

        }

        public bool testConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }

        }

        public DataTable getDbnames()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'model', 'msdb', 'tempdb')";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public bool Backup(string dbName, bool doesExist)
        {
            try
            {
                string newPath = Path.Combine(this.backupPath, dbName);

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = string.Empty;

                    if (doesExist)
                    {
                        query = $"BACKUP DATABASE {dbName} TO DISK = '{newPath}.bak' " +
                        $"WITH DIFFERENTIAL, NAME = 'Partial Backup of {dbName}', DESCRIPTION = 'Partial Database Backup'";
                    }

                    else
                    {
                        query = $"BACKUP DATABASE {dbName} TO DISK = '{newPath}.bak' " +
                        $"WITH NAME = 'Full Backup of {dbName}', DESCRIPTION = 'Full Database Backup'";
                    }
                    

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }

                }

                return true;
            }

            catch (SqlException)
            {
                return false;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public bool restoreDatabase(string dbName)
        {
            try
            {
                string newPath = Path.Combine(this.backupPath, dbName);

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = $"IF Exists (SELECT name FROM sys.databases WHERE name = '{dbName}') " +
                        $"BEGIN " +
                        $"DROP DATABASE {dbName} " +
                        $"END " +
                        $"RESTORE DATABASE {dbName} FROM DISK = '{newPath}.bak'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }

                }

                return true;
            }

            catch (SqlException)
            {
                return false;
            }

            catch (Exception)
            {
                return false;
            }
        }

        public bool doesBackupExist(string dbName)
        {
            string filePath = Path.Combine(this.backupPath, $"{dbName}.bak");

            if (File.Exists(filePath))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
