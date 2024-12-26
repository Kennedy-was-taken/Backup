using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace Backup
{
    public class Validate
    {
        public ServiceResponse<bool> isSqlServerInstalled()
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
