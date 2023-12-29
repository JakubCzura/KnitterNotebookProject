using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.HelpersForTesting;

public class DatabaseHelper
{
    /// <summary>
    /// Connection string for the database used in integration tests
    /// </summary>
    private static string ConnectionString => $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KnitterNotebookDbIntegrationTests;Integrated Security=True;";

    /// <summary>
    /// Creates a new database context for the integration tests
    /// </summary>
    /// <returns>Instance of DatabaseContext for integration tests</returns>
    public static DatabaseContext CreateDatabaseContext()
        => new(new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(ConnectionString).Options);

    /// <summary>
    /// Creates a new empty database for the integration tests
    /// </summary>
    /// <param name="databaseContext">DatabaseContext with details to create database</param>
    public static void CreateEmptyDatabase(DatabaseContext databaseContext)
    {
        databaseContext.Database.EnsureDeleted();
        databaseContext.Database.Migrate();
    }
}