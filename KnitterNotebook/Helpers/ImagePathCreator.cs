using KnitterNotebook.ApplicationInformation;
using System;
using System.IO;

namespace KnitterNotebook.Helpers
{
    public class ImagePathCreator
    {
        public static string? CreateUniquePathToSaveImage(string? userName, string? sourceImageFullPath)
        {
            return string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(sourceImageFullPath)
                ? null
                : Paths.PathToSaveImage(userName, DateTime.Now.Ticks.ToString(), Path.GetFileName(sourceImageFullPath));
        }
    }
}