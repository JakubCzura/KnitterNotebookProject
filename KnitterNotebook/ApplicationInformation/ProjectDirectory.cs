using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ApplicationInformation
{
    public class ProjectDirectory
    {
        /// <summary>
        /// Path to project's directory
        /// </summary>
        public static string ProjectDirectoryFullPath { get => Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName); }
    }
}
