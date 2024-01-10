using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.HelpersForTesting;

public class DatabaseHelper
{
    /// <summary>
    /// Connection string for the database used in integration tests
    /// </summary>
    private static string ConnectionString => $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={Guid.NewGuid()};Integrated Security=True;";

    /// <summary>
    /// Creates a new database context for the integration tests
    /// </summary>
    /// <returns>Instance of DatabaseContext for integration tests</returns>
    public static DatabaseContext CreateDatabaseContext()
        => new(new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(ConnectionString).Options);
}