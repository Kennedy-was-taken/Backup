﻿using Backup.Databases.MSSQL;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Backup
{
    public class Init
    {

        private List<string>? dbNames;

        private readonly IConfiguration configuration;

        public Init(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [ExcludeFromCodeCoverage]
        public void begin()
        {
            PopulateDatabaseList();

            DisplayDatabases();
        }

        public void PopulateDatabaseList()
        {
            dbNames = new List<string>();
            var Databases = configuration.GetSection("Databases");

            foreach (var item in Databases.GetChildren())
            {
                dbNames.Add(item.Key);
            }

        }

        [ExcludeFromCodeCoverage]
        private void DisplayDatabases()
        {
            int response = 0;

            if (dbNames == null)
            {
                Console.WriteLine("No Databases found : ");
            }

            else
            {

                Console.WriteLine("Current Databases Available : ");
                for (int i = 0; i < dbNames?.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {dbNames[i]}");
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Select you option (e.g. 1) : ");

                        var userInput = Console.ReadLine();

                        if (userInput != null)
                        {
                            response = int.Parse(userInput.ToString());

                        }

                        if (response <= dbNames?.Count && response != 0)
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

                ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

                switch (dbNames[response - 1])
                {
                    case "MySql":

                        break;

                    case "Postgre":

                        break;

                    case "SQL Server":
                        Console.WriteLine("");
                        serviceResponse = Validate.isSqlServerInstalled();

                        if (serviceResponse.isSuccess)
                        {
                            Console.WriteLine(serviceResponse.message);
                            DatabaseOption(dbNames[response - 1]);

                        }

                        else
                        {
                            Console.WriteLine(serviceResponse.message);
                        }
                        break;
                }

            }
        }

        [ExcludeFromCodeCoverage]
        public void DatabaseOption(string dbName)
        {
            int response = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine($"Would you like to backup or restore : ");
                    Console.WriteLine("1. Backup Database");
                    Console.WriteLine("2. Restore Database");
                    Console.Write($"Select you option (e.g. 1) for {dbName}: ");

                    var userInput = Console.ReadLine();

                    if (userInput != null)
                    {
                        response = int.Parse(userInput.ToString());
                    }

                    if (response <= dbNames?.Count && response != 0)
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

            if (response == 1)
            {
                Redirector(dbName, "Backup");
            }

            else
            {
                Redirector(dbName, "Restore");
            }

            
        }

        [ExcludeFromCodeCoverage]
        public void Redirector(string dbName, string option)
        {
            switch (dbName)
            {
                case "MySql":

                break;

                case "Postgre":

                break;

                case "SQL Server":

                    MssqlService mssqlservice = new(configuration);

                    Console.WriteLine("");
                    Console.WriteLine($"Testing Connnection to {dbName}");
                    var isConnected = mssqlservice.TestConnection();

                    if (isConnected.isSuccess)
                    {
                        Console.WriteLine(isConnected.message);
                        mssqlservice.startProcess(option);
                    }

                    else
                    {
                        Console.WriteLine(isConnected.message);
                    }

                break;

                default:
                    Console.WriteLine("No database with that name exists");
                break;
            }
        }

    }
}
