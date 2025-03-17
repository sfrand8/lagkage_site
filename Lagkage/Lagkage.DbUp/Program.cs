using System.Reflection;
using DbUp;
using DbUp.Engine;

internal class Program
{
    static int Main(string[] args)
    {
        var connectionString =
            args.FirstOrDefault()
            ?? "Host=localhost;Port=5432;Database=mydatabase;Username=admin;Password=secret;SearchPath=lagkage";

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