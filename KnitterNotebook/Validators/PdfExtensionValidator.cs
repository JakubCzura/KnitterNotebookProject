using System;
using System.IO;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class PdfExtensionValidator
    {
        public static bool IsPdf(string? filePath)
        {
            string? extension = Path.GetExtension(filePath);
            string[] validExtensions = { ".pdf" };
            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}