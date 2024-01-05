using KnitterNotebook.Models.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace KnitterNotebook.ApplicationInformation;

public class Paths
{
    /// <summary>
    /// Path to project's directory
    /// </summary>
    public static string ProjectDirectory 
        => Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName);

    /// <summary>
    /// Path to directory with users' directories
    /// </summary>
    public static string UsersDirectories 
        => Path.Combine(SpecialDirectories.MyDocuments, "KnitterNotebook", "UsersDirectories");

    /// <summary>
    /// Path to directory of user with <paramref name="nickname"/>
    /// </summary>
    /// <param name="nickname">User's nickname</param>
    public static string UserDirectory(string nickname) 
        => Path.Combine(UsersDirectories, nickname);

    /// <summary>
    /// If <paramref name="nickname"/> or <paramref name="fileNameWithExtension"/> is null the function returns null and it is expected behaviour as entities sometimes have user's file's path property that is not required.
    /// <para>Each path is intentionally unique to avoid problems with duplicated items</para>
    /// </summary>
    /// <param name="nickname">Nickname of user</param>
    /// <param name="fileNameWithExtension">Full path to file with extension that can be saved</param>
    /// <returns>Null if <paramref name="nickname"/> or <paramref name="fileNameWithExtension"/> is null otherwise full path to save user's file</returns>
    public static string? PathToSaveUserFile(string? nickname, string? fileNameWithExtension)
         => string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(fileNameWithExtension)
             ? null
             : Path.Combine(UserDirectory(nickname), DateTime.Now.Ticks.ToString() + fileNameWithExtension);

    /// <summary>
    /// Returns path to resource dictionary that contains application's theme specified by <paramref name="themeName"/>
    /// </summary>
    /// <param name="themeName">Name of application's theme</param>
    /// <returns>Path to resource dictionary of application's theme</returns>
    public static string Theme(ApplicationTheme themeName) 
        => Path.Combine(ProjectDirectory, $"Themes/{themeName}Mode.xaml");

    /// <summary>
    /// Name of file with application's settings
    /// </summary>
    public static string AppSettings => "appsettings.json";
}