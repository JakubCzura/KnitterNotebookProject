namespace KnitterNotebook.IntegrationTests.HelpersForTesting
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Creates unique name of database
        /// </summary>
        public static string CreateUniqueDatabaseName => "TestDb" + Guid.NewGuid().ToString();
    }
}