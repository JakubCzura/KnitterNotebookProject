using System;
using System.IO;

namespace KnitterNotebook.ApplicationInformation
{
    public class SolutionDirectory
    {
        /// <summary>
        /// Path to solution's directory
        /// </summary>
        public static string SolutionDirectoryFullPath => Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName);
    }
}