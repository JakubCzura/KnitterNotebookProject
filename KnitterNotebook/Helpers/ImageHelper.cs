using KnitterNotebook.ApplicationInformation;
using System.IO;

namespace KnitterNotebook.Helpers
{
    public class ImageHelper
    {
        public static string? CreatePathToSaveImage(string userName, string? sourceImageFullPath)
        {
            return string.IsNullOrWhiteSpace(sourceImageFullPath) ? null : Paths.ImageToSavePath(userName, Path.GetFileName(sourceImageFullPath));
        }
    }
}