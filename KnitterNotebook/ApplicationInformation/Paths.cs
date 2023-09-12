using KnitterNotebook.Models.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace KnitterNotebook.ApplicationInformation
{
    public class Paths
    {
        public static string UsersDirectories => Path.Combine(SpecialDirectories.MyDocuments, "KnitterNotebook", "UsersDirectories");

        public static string UserDirectory(string nickname) => Path.Combine(UsersDirectories, nickname);

        /// <summary>
        /// If <paramref name="nickname"/> or <paramref name="fileNameWithExtension"/> is null the function returns null and it is expected behaviour as sometimes entities have user's file's path property that is not required.
        /// </summary>
        /// <param name="nickname">Nickname of user</param>
        /// <param name="fileNameWithExtension">Full path to file with extension that can be saved</param>
        /// <returns>Null if <paramref name="nickname"/> or <paramref name="fileNameWithExtension"/> is null otherwise full path to save file</returns>
        public static string? PathToSaveUserFile(string? nickname, string? fileNameWithExtension)
             => string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(fileNameWithExtension)
             ? null
             : Path.Combine(UserDirectory(nickname), DateTime.Now.Ticks.ToString() + fileNameWithExtension);

        public static string ThemeFullPath(ApplicationTheme themeName) => Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{themeName}Mode.xaml");
    }
}