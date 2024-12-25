using Microsoft.Extensions.Configuration;
using System.Data;

namespace Backup.Databases.MSSQL
{
    public class MssqlService
    {
        //global variables
        private readonly IConfiguration configuration;
        private readonly MssqlRepository repository;

        public MssqlService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repository = new MssqlRepository(configuration);
        }

        public ServiceResponse<bool> TestConnection()
        {
            ServiceResponse<bool> service = new();

            var isConnected = repository.testConnection();

            if (isConnected)
            {
                service.data = isConnected;
                service.message = "Successfully connected to Sql Server";
                service.isSuccess = isConnected;
            }

            else
            {
                service.data = isConnected;
                service.message = "Failed to connect to Sql Server";
                service.isSuccess = isConnected;
            }

            return service;
        }

        // Gets Database names
        public ServiceResponse<DataTable> DatabaseNames()
        {
            ServiceResponse<DataTable> service = new();

            var dbNames = repository.getDbnames();

            if (dbNames != null)
            {
                service.data = dbNames;
                service.message = "Databases were found";
                service.isSuccess = true;
            }

            else
            {
                service.data = null;
                service.message = "No databases were found";
                service.isSuccess = false;
            }

            return service;
        }

        // Gets if a backup database exists
        public ServiceResponse<bool> CheckBackupExists(string dbName)
        {
            ServiceResponse<bool> service = new();

            bool doesExist = repository.doesBackupExist(dbName);

            if (doesExist)
            {
                service.data = doesExist;
                service.message = $"Backup {dbName} database was found";
                service.isSuccess = doesExist;
            }

            else
            {
                service.data = doesExist;
                service.message = $"There isn't a Backup {dbName} database";
                service.isSuccess = doesExist;
            }

            return service;
        }

        public ServiceResponse<bool> BackupDatabase(string dbName, bool result)
        {
            ServiceResponse<bool> service = new();

            bool results = repository.Backup(dbName, result);

            if (results)
            {
                service.data = results;
                service.message = $"{dbName} database was has been backed up";
                service.isSuccess = results;
            }

            else
            {
                service.data = results;
                service.message = $"{dbName} database could not be backed up";
                service.isSuccess = results;
            }

            return service;
        }

        public ServiceResponse<bool> RestoreDatabase(string dbName)
        {
            ServiceResponse<bool> service = new();

            bool results = repository.restoreDatabase(dbName);

            if (results)
            {
                service.data = results;
                service.message = $"{dbName} database was has been restored";
                service.isSuccess = results;
            }

            else
            {
                service.data = results;
                service.message = $"{dbName} database failed to be restored";
                service.isSuccess = results;
            }

            return service;
        }

    }

}
