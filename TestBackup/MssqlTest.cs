using Microsoft.Extensions.Configuration;
using Xunit;
using System.IO;
using Backup.Databases.MSSQL;

namespace TestBackup
{
    public class MssqlTest
    {
        private IConfiguration? configuration;

        MssqlRepository repository;

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
    }
}
