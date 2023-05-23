using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Helpers
{
    public class ImageHelper
    {
        public static bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}
