using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    public class Commands
    {
        protected Commands()
        {
            
        }

        public static void Director(string command, IConfiguration configuration)
        {
            if (command.Equals("--help"))
            {
                Help();
            }

            if (command.Contains('-') && command != "--help" )
            {
                Help();
            }

            try
            {
                Init i = new Init(configuration);
                i.PopulateDatabaseList();
                i.selectedDatabase(int.Parse(command));
            }

            catch (FormatException)
            {
                Console.WriteLine("your argument was not a numerical value");
            }

        }

        private static void Help()
        {
            Console.WriteLine("Backup : to begin the program");
            Console.WriteLine("Backup <n> : if you already know the sequence number of your database, enter that value");
        }
    }
}
