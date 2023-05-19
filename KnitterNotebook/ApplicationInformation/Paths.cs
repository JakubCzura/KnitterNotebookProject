using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ApplicationInformation
{
    public class Paths
    {
        public static string UsersDirectories => Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "UsersDirectories");
        public static string UserDirectory(string userName) => Path.Combine(UsersDirectories, userName);
    }
}
