using System.Data;
using System.Diagnostics;
using Dapper;
using Npgsql;

namespace Lagkage.Integration.Tests;
public class DatabaseFixture : IDisposable
{
    private readonly string _connectionString;
    private readonly string _testDbName = "lagkage_test_database"; // Define test DB name separately
    
    public IDbConnection Connection => new NpgsqlConnection(_connectionString + $"Database={_testDbName}");
    private string ConnectionString => _connectionString + $"Database={_testDbName};";

    public DatabaseFixture()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        // Define connection string for your PostgreSQL server
        _connectionString = "Host=localhost;Port=5432;Username=admin;Password=secret;";

        // Ensure the test database exists and is ready
        CreateTestDatabase();
        
        // Run migrations
        RunMigrations();
    }

    public bool CheckDatabaseExists()
    {
        using (var connection = new NpgsqlConnection(_connectionString  +  "Database=postgres;"))
        {
            connection.Open();
            
            // Query the pg_database table to see if the database exists
            var result = connection.Query<int>(
                "SELECT 1 FROM pg_database WHERE datname = @DbName",
                new { DbName = _testDbName }).FirstOrDefault();

            return result == 1;
        }
    }
    
    // Helper method to create the test database
    private void CreateTestDatabase()
    {
        // Connect to the default "postgres" database to create "mydatabase"
        var connectionStringForCreateDb = _connectionString + "Database=postgres;"; 

        using (var connection = new NpgsqlConnection(connectionStringForCreateDb))
        {
            connection.Open();

            if (!CheckDatabaseExists()) // Only create if it doesn't already exist
            {
                // Create the database
                using (var cmd = new NpgsqlCommand($"CREATE DATABASE {_testDbName};", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
    
    private void RunMigrations()
    {
        var result = DbUpMigrator.RunMigrations(ConnectionString);

        if (result.Successful == false)
        {
            throw new Exception($"Migration failed: {result.Error}");
        }
    }

    // Clean up by truncating the tables after tests
    public void Dispose()
    {
        TruncateAllTables();
    }
    
    public void TruncateAllTables()
    {
        // Connect to the "mydatabase" for truncating the tables
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();

            // Get the list of all tables
            var tables = connection.Query<string>("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';").ToList();

            // Truncate each table
            foreach (var table in tables)
            {
                using (var cmd = new NpgsqlCommand($"TRUNCATE TABLE {table} RESTART IDENTITY CASCADE;", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
