using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.HelpersForTesting
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Creates unique name of database
        /// </summary>
        private static string CreateUniqueDatabaseName => $"KnitterNotebookDbIntegrationTests{DateTime.Now.Ticks}";

        /// <summary>
        /// I decided to use unique database's name as I can run many tests in parallel without any problems
        /// </summary>
        private static string ConnectionString => $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={CreateUniqueDatabaseName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static DatabaseContext CreateDatabaseContext()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseSqlServer(ConnectionString);
            return new DatabaseContext(builder.Options);
        }
    }
}