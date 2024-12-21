using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Backup.Databases.MSSQL
{
    public class MssqlRepository
    {
        private readonly IConfiguration configuration;

        public MssqlRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private string? GetConnectionString()
        {
            return configuration.GetSection("Databases:SQL Server").Value;
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
    }
}
