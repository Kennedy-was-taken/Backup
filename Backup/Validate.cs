using Microsoft.SqlServer.Management.Smo;
using System.Diagnostics.CodeAnalysis;

namespace Backup
{
    public class Validate
    {
        [ExcludeFromCodeCoverage]
        protected Validate()
        {

        }
        public static ServiceResponse<bool> isSqlServerInstalled()
        {
            ServiceResponse<bool> service = new ServiceResponse<bool>();
            try
            {
                Server server = new Server();

                service.data = true;
                service.message = "Sql Server found on the System";
                service.isSuccess = true;
            }

            catch
            {
                service.data = false;
                service.message = "Sql Server was not found on the System";
                service.isSuccess = false;
            }

            return service;
        }
    }
}
