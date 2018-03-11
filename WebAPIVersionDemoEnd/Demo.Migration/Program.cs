using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Demo.Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();

            IConfiguration config = configBuilder.Build();
            var connectionString = config.GetConnectionString("demoDb");

            EnsureDatabase.For.SqlDatabase(connectionString);
            var upgrader =
               DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithVariable("userpassword", "web_api_demo!12345")
                    .WithScriptsFromFileSystem("./Scripts/2000_Schemas")
                    .WithScriptsFromFileSystem("./Scripts/3000_Logins")
                    .WithScriptsFromFileSystem("./Scripts/4000_Tables")
                    .JournalToSqlTable("Demo", "Version") //version schema binding				   
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
