using KnitterNotebook.ApplicationInformation;
using System.IO;

namespace KnitterNotebook.Helpers
{
    public class ImageHelper
    {
        public static string? CreatePathToSaveImage(string userName, string sourceImageFullPath)
        {
            return string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(sourceImageFullPath) 
                ? null 
                : Paths.PathToSaveImage(userName, Path.GetFileName(sourceImageFullPath));
        }
    }
}