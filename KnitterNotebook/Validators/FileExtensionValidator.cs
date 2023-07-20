using System;
using System.IO;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class FileExtensionValidator
    {
        public static bool IsPdf(string? filePath)
        {
            string[] validExtensions = { ".pdf" };
            return IsExtensionValid(filePath, validExtensions);
        }

        public static bool IsImage(string? filePath)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return IsExtensionValid(filePath, validExtensions);
        }

        private static bool IsExtensionValid(string? filePath, string[] allowedExtensions)
        {
            string? extension = Path.GetExtension(filePath);
            return allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}