namespace KnitterNotebookTests.HelpersForTesting
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Creates unique name of database
        /// </summary>
        public static string CreateUniqueDatabaseName => "TestDb" + DateTime.Now.Ticks.ToString();
    }
}