using System.Reflection;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;

internal class Program
{
    static int Main(string[] args)
    {
        // Set up the configuration to read from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) 
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) 
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var result = DbUpMigrator.RunMigrations(connectionString);

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
    #if DEBUG
            Console.ReadLine();
    #endif                
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return 0;
    }
}

public static class DbUpMigrator
{
    public static DatabaseUpgradeResult RunMigrations(string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        return upgrader.PerformUpgrade();
    }
}