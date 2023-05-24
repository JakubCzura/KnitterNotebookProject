using KnitterNotebook.ApplicationInformation;
using System.IO;

namespace KnitterNotebook.Helpers
{
    public class ImageHelper
    {
        public static string? CreateImageToSavePath(string userName, string? sourceImageName)
        {
            return string.IsNullOrWhiteSpace(sourceImageName) ? null : Paths.ImageToSavePath(userName, Path.GetFileName(sourceImageName));
        }
    }
}