using System;
using System.IO;

namespace KnitterNotebook.ApplicationInformation
{
    public class ProjectDirectory
    {
        /// <summary>
        /// Path to project's directory
        /// </summary>
        public static string ProjectDirectoryFullPath => Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName);
    }
}