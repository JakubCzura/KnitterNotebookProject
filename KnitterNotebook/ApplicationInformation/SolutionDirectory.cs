using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ApplicationInformation
{
    public class SolutionDirectory
    {
        /// <summary>
        /// Path to solution's directory
        /// </summary>
        public static string SolutionDirectoryFullPath { get => Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName); }
    }
}
