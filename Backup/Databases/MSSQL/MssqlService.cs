using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

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

        [ExcludeFromCodeCoverage]
        public void startProcess(string option)
        {

            switch (option)
            {
                case "Backup":
                    ServiceResponse<DataTable> backupService = DatabaseNames();
                    display(backupService, option);
                break;

                case "Restore":
                    ServiceResponse<List<string>> restoreService = getDatabaseBackupFiles();
                    display(restoreService, option);
                break;

            }
        }

        [ExcludeFromCodeCoverage]
        public void display<T>(ServiceResponse<T> dbName, string option)
        {
            int response = 0;

            dynamic? data = dbName.data;

            switch (option)
            {
                case "Backup":

                    while (true)
                    {
                        try
                        {

                            int count = 0;

                            foreach (DataRow row in data.Rows)
                            {
                                Console.Write($"{count + 1}. {row["name"].ToString()}");
                                count++;
                            }

                            Console.Write("Select you option (e.g. 1) : ");

                            var userInput = Console.ReadLine();

                            if (userInput != null)
                            {
                                response = int.Parse(userInput.ToString());

                                if (response == 0)
                                {
                                    throw new Exception();
                                }
                            }

                            else
                            {
                                throw new Exception();
                            }

                            if (response <= data?.Rows.Count)
                            {
                                break;
                            }

                            else
                            {
                                Console.WriteLine("please enter a numerical value from the list");
                            }
                        }

                        catch (InvalidCastException)
                        {
                            Console.WriteLine("please enter a numerical value from the list");

                        }

                        catch (Exception)
                        {
                            Console.WriteLine("please enter a numerical value from the list");
                        }
                    }

                    if (data["name"][response].ToString() == data["name"][response].ToString())
                    {
                        BackupOrRestore(data["name"][response - 1], option);
                    }

                    else
                    {
                        BackupOrRestore(data, option);
                    }

                    break;

                case "Restore":

                    while (true)
                    {
                        try
                        {

                            for (int i = 0; i < data?.Count; i++)
                            {
                                Console.Write($"{i + 1}. {data[i]}");
                            }

                            Console.Write("Select you option (e.g. 1) : ");

                            var userInput = Console.ReadLine();

                            if (userInput != null)
                            {
                                response = int.Parse(userInput.ToString());

                                if (response == 0)
                                {
                                    throw new Exception();
                                }
                            }

                            else
                            {
                                throw new Exception();
                            }

                            if (response <= data.Count)
                            {
                                break;
                            }

                            else
                            {
                                Console.WriteLine("please enter a numerical value from the list");
                            }
                        }

                        catch (InvalidCastException)
                        {
                            Console.WriteLine("please enter a numerical value from the list");

                        }

                        catch (Exception)
                        {
                            Console.WriteLine("please enter a numerical value from the list");
                        }
                    }

                    if (data[data.Count - 1].ToString() == data[response].ToString())
                    {
                        BackupOrRestore(data[response - 1], option);
                    }

                    else
                    {
                        BackupOrRestore(data, option);
                    }

                break;

            }
        }

        // backs up a single data
        [ExcludeFromCodeCoverage]
        public void BackupOrRestore(string dbName, string option)
        {
            switch (option)
            {
                case "Backup":

                    var result = CheckBackupExists(dbName);

                    Console.WriteLine($"Attempting to Backup {dbName}");

                    var backupResult = BackupDatabase(dbName, result.data);
                   
                    Console.WriteLine(backupResult.message);
                    
                break;

                case "Restore":

                    Console.WriteLine($"Attempting to Restore {dbName}");

                    var restoreResult = RestoreDatabase(dbName);

                    Console.WriteLine(restoreResult.message);

                break;

            }
        }

        //overloading method
        ///backing up multiple data
        [ExcludeFromCodeCoverage]
        public void BackupOrRestore<T>(T dbName, string option)
        {
            //inserting dbName into a dynamic type
            dynamic? obj = dbName;

            switch (option)
            {
                case "Backup":

                    foreach (DataRow row in obj.Rows)
                    {
                        var result = CheckBackupExists(obj["name"].ToString());

                        Console.WriteLine($"Attempting to Backup {obj["name"].ToString()}");

                        var restoreResult = BackupDatabase(obj["name"].ToString(), result.data);

                        Console.WriteLine(restoreResult.message);
                    }

                    break;

                case "Restore":

                    for (int i = 0; i < obj.Count; i++)
                    {
                        Console.WriteLine($"Attempting to Restore {obj[i].ToString()}");

                        var backupResult = RestoreDatabase(obj[i].ToString());

                        Console.WriteLine(backupResult.message);
                    }
                    break;

            }
        }

        public ServiceResponse<List<string>> getDatabaseBackupFiles()
        {
            ServiceResponse<List<string>> service = new();
            var results = repository.getBackupFileName();

            if (results != null)
            {
                if (results.Length >= 2)
                {
                    var newType = results.ToList();
                    newType.Add("Restore all");
                    service.data = newType;
                }

                else
                {
                    service.data = results.ToList();
                }

                service.message = "Files found";
                service.isSuccess = true;

            }

            else
            {
                service.data = null;
                service.message = "Files not found";
                service.isSuccess = false;
            }

            return service;
        }

        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public ServiceResponse<DataTable> DatabaseNames()
        {
            ServiceResponse<DataTable> service = new();

            var dbNames = repository.getDbnames();

            if (dbNames != null)
            {
                // checks it contains more than two items
                if (dbNames.Rows.Count >= 2)
                {
                    DataRow newRow = dbNames.NewRow();
                    newRow["name"] = "Backup All";
                    dbNames.Rows.Add(newRow);
                }

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
