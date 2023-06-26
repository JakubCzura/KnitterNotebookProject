using KnitterNotebook.ApplicationInformation;
using System.IO;

namespace KnitterNotebook.Helpers
{
    public class ImagePathCreator
    {
        public static string? CreatePathToSaveImage(string userName, string sourceImageFullPath)
        {
            return string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(sourceImageFullPath) 
                ? null 
                : Paths.PathToSaveImage(userName, Path.GetFileName(sourceImageFullPath));
        }
    }
}