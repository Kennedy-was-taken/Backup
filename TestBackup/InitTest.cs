using Backup;
using Backup.Databases.MSSQL;
using Microsoft.Extensions.Configuration;

namespace TestBackup
{
    public class InitTest
    {
        private IConfiguration? configuration;

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

        [Fact]
        public void testPopulateDatabaseList()
        {
            try
            {
                setConfiguration();

                if (configuration != null)
                {
                    Init start = new(configuration);
                    start.PopulateDatabaseList();
                    Assert.True(true);
                }

                else
                {
                    Assert.True(false);
                }
                
            }

            catch (Exception)
            {
                Assert.Fail();
            }
        }


    }
}
