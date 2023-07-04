using System;
using System.IO;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class ImageExtensionValidator
    {
        public static bool IsImage(string? filePath)
        {
            string? extension = Path.GetExtension(filePath);
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}