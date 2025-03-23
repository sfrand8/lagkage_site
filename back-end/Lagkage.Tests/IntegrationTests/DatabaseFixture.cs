using System;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class DatabaseFixture : IDisposable
{
    private readonly string _baseConnectionString;
    private readonly string _testDbName = "lagkage_test_database"; // Define test DB name separately

    public IDbConnection Connection => new NpgsqlConnection(TestDatabaseConnectionString);

    private string TestDatabaseConnectionString => $"{_baseConnectionString};Database={_testDbName};";
    private string PostgresDatabaseConnectionString => $"{_baseConnectionString};Database=postgres;";

    public DatabaseFixture()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        // Load the connection string from appsettings.Development.json
        _baseConnectionString = GetBaseConnectionString();

        // Ensure the test database exists and is ready
        CreateTestDatabase();

        // Run migrations
        RunMigrations();
    }

    private string GetBaseConnectionString()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        
        string appSettingsFile = $"appsettings.{environment}.json";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure correct path
            .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception($"Connection string 'DefaultConnection' not found in {appSettingsFile}");
        }

        return RemoveDatabaseFromConnectionString(connectionString);
    }

    private string RemoveDatabaseFromConnectionString(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        builder.Remove("Database"); // Remove database key if present
        return builder.ToString();
    }

    public bool CheckDatabaseExists()
    {
        using var connection = new NpgsqlConnection(PostgresDatabaseConnectionString);
        connection.Open();

        return connection.Query<int>(
            "SELECT 1 FROM pg_database WHERE datname = @DbName",
            new { DbName = _testDbName }).FirstOrDefault() == 1;
    }

    private void CreateTestDatabase()
    {
        using var connection = new NpgsqlConnection(PostgresDatabaseConnectionString);
        connection.Open();

        if (!CheckDatabaseExists())
        {
            using var cmd = new NpgsqlCommand($"CREATE DATABASE {_testDbName};", connection);
            cmd.ExecuteNonQuery();
        }
    }

    private void RunMigrations()
    {
        var result = DbUpMigrator.RunMigrations(TestDatabaseConnectionString);

        if (!result.Successful)
        {
            throw new Exception($"Migration failed: {result.Error}");
        }
    }

    public void Dispose()
    {
        TruncateAllTables();
    }

    public void TruncateAllTables()
    {
        using var connection = new NpgsqlConnection(TestDatabaseConnectionString);
        connection.Open();

        var tables = connection.Query<string>(
            "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';"
        ).ToList();

        foreach (var table in tables)
        {
            using var cmd = new NpgsqlCommand($"TRUNCATE TABLE {table} RESTART IDENTITY CASCADE;", connection);
            cmd.ExecuteNonQuery();
        }
    }
}
