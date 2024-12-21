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

        public static void Director(string command)
        {
            if (command.Equals("--help"))
            {
                Help();
            }

            if (command.Contains('-') && command != "--help" )
            {
                Help();
            }

        }

        private static void Help()
        {
            Console.WriteLine("Backup : to begin the program");
            Console.WriteLine("Backup <n> : if you already know the sequence number of your database, enter that value");
        }
    }
}
