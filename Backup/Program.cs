﻿using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Backup
{
    public class Program
    {
        protected Program()
        {
            
        }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            //sets the path
            var backtrack = Path.Combine("..", "..", "..");
            var jsonpath = Path.Combine(Directory.GetCurrentDirectory(), backtrack);

            //reads from the appsetting.json
            builder.SetBasePath(jsonpath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            Init i = new Init(configuration);
            i.begin();    

        }

    }
}