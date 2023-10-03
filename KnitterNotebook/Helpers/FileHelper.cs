using System;
using System.IO;

namespace KnitterNotebook.Helpers;

public static class FileHelper
{
    /// <summary>
    /// Creates directory for <paramref name="destinationFileName"/> if it doesn't exist and copies <paramref name="sourceFileName"/> to it
    /// </summary>
    /// <param name="sourceFileName">Full path of file to copy</param>
    /// <param name="destinationFileName">Full path where to copy the file</param>
    /// <param name="overwrite">True if existing file can be overwritten, otherwise false</param>
    ///<exception cref="ArgumentNullException">When <paramref name="sourceFileName"/> or <paramref name="destinationFileName"/> is null</exception>"
    ///<exception cref="DirectoryNotFoundException">When <paramref name="sourceFileName"/> consists directory that doesn't exist/exception>"
    ///<exception cref="FileNotFoundException">When <paramref name="sourceFileName"/> doesn't exist/exception>"
    public static void CopyWithDirectoryCreation(string sourceFileName, string destinationFileName, bool overwrite = false)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationFileName)!);
        File.Copy(sourceFileName, destinationFileName, overwrite);
    }
}