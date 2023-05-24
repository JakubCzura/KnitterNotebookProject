using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Models;
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
        public static string? CreateImageToSavePath(string userName, string? sourceImageName)
        {
            return string.IsNullOrWhiteSpace(sourceImageName) ? null : Paths.ImageToSavePath(userName, Path.GetFileName(sourceImageName));
        }
    }
}
