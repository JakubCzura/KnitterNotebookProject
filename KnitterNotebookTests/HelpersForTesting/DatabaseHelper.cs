using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebookTests.HelpersForTesting
{
    public class DatabaseHelper
    {
        public static string CreateUniqueDatabaseName => "TestDb" + DateTime.Now.Ticks.ToString();
    }
}
