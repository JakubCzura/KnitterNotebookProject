using System.IO;

namespace KnitterNotebook.ApplicationInformation
{
    public class Paths
    {
        public static string UsersDirectories => Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "UsersDirectories");
        //public static string UsersDirectories => Path.Combine(SpecialDirectories.MyDocuments, "UsersDirectories");

        public static string UserDirectory(string nickname) => Path.Combine(UsersDirectories, nickname);

        public static string PathToSaveImage(string nickname, string uniqueString, string sourceImageName) => Path.Combine(UserDirectory(nickname), uniqueString + sourceImageName);

        public static string ThemeFullPath(string themePath) => Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{themePath}Mode.xaml");
    }
}