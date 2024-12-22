using Microsoft.Extensions.Configuration;
using Xunit;
using System.IO;
using Backup.Databases.MSSQL;
using System.Data;

namespace TestBackup
{
    public class MssqlTest
    {
        private IConfiguration? configuration;

        MssqlRepository repository;

        DataTable dt;

        private void setConfiguration()
        {
            var builder = new ConfigurationBuilder();

            //sets the path
            var backtrack = Path.Combine("..", "..", "..", "..", "Backup");
            var jsonpath = Path.Combine(Directory.GetCurrentDirectory(), backtrack);

            //reads from the appsetting.json
            builder.SetBasePath(jsonpath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();

        }

        //[Fact]
        //public void getConnectionString()
        //{
        //    setConfiguration();
        //    repository = new MssqlRepository(configuration);

        //    var connectionString = repository.GetConnectionString();

        //    Assert.True(connectionString != null);

        //}

        [Fact]
        public void testConnection()
        {
            setConfiguration();
            repository = new MssqlRepository(configuration);

            bool isConnected = repository.testConnection();
            Assert.True(isConnected);
        }

        [Fact]
        public void retrieveDbName()
        {
            setConfiguration();
            repository = new MssqlRepository(configuration);

            dt = new DataTable();

            dt = repository.getDbnames();

            if (dt == null)
            {
                Assert.True(false);
            }

            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Console.WriteLine(dr.ToString());
                }
                Assert.True(true);
            }
        }

        [Fact]
        public void testFullBackup()
        {
            setConfiguration();
            repository = new MssqlRepository(configuration);

            dt = new DataTable();

            dt = repository.getDbnames();

            if (repository.fullBackup(dt.Rows[0]["name"].ToString()))
            {
                Assert.True(true);
            }

            else
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void differentialFullBackup()
        {
            setConfiguration();
            repository = new MssqlRepository(configuration);

            dt = new DataTable();

            dt = repository.getDbnames();

            if (repository.differentialBackup(dt.Rows[0]["name"].ToString()))
            {
                Assert.True(true);
            }

            else
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void testRestore()
        {
            setConfiguration();
            repository = new MssqlRepository(configuration);

            dt = new DataTable();

            dt = repository.getDbnames();

            bool isRestored = repository.restoreDatabase(dt.Rows[0]["name"].ToString());

            Assert.True(isRestored);

        }
    }
}
