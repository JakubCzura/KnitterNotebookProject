using System;
using System.IO;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class FileExtensionValidator
    {
        private static readonly string[] validPdfExtensions = { ".pdf" };
        private static readonly string[] validImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        public static bool IsPdf(string? filePath) => IsExtensionValid(filePath, validPdfExtensions);

        public static bool IsImage(string? filePath) => IsExtensionValid(filePath, validImageExtensions);

        private static bool IsExtensionValid(string? filePath, string[] allowedExtensions)
            => allowedExtensions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase);
    }
}