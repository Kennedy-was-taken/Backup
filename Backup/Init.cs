using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    public class Init
    {

        List<string> dbNames;
        bool moreThanOne = false;

        private readonly IConfiguration configuration;

        public Init(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void begin()
        {
            PopulateDatabaseList(configuration);

            //DisplayDatabases();
        }

        private void PopulateDatabaseList(IConfiguration configuration)
        {
            dbNames = new List<string>();
            var Databases = configuration.GetSection("Databases");

            foreach (var item in Databases.GetChildren())
            {
                dbNames.Add(item.Key);
            }

            if (dbNames.Count > 1)
            {
                dbNames.Add("Backup All");
                moreThanOne = true;
            }

        }

        private void DisplayDatabases()
        {
            int response = 0;

            Console.WriteLine("Current Databases Available : ");
            for (int i = 0; i < dbNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dbNames[i]}");
            }

            while (true)
            {
                try
                {
                    Console.Write("Select you option (e.g. 1) : ");

                    response = int.Parse(Console.ReadLine());

                    if (response <= dbNames.Count)
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("please enter a numerical value from the list");
                    }
                }

                catch (InvalidCastException ex)
                {
                    Console.WriteLine("please enter a numerical value from the list");

                }
            }

            if (moreThanOne)
            {
                if (dbNames[response - 1] == "Backup All")
                {
                    Console.WriteLine("Backing up all databases");
                }

                else
                {
                    Console.WriteLine($"Backing up {dbNames[response - 1].ToString()}");
                }
            }

            else
            {
                Console.WriteLine($"Backing up {dbNames[response - 1].ToString()}");
            }

        }
    }
}
