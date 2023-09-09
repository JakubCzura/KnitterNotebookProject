﻿using KnitterNotebook.Database;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.HelpersForTesting
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Creates unique name of database
        /// </summary>
        public static string CreateUniqueDatabaseName => "TestDb" + Guid.NewGuid().ToString();

        private static string ConnectionString => "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KnitterNotebookDbIntegrationTests;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static DatabaseContext CreateDatabaseContext()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseSqlServer(ConnectionString);
            return new DatabaseContext(builder.Options);
        }
    }
}