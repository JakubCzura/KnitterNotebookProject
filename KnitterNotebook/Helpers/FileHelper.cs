using System.IO;

namespace KnitterNotebook.Helpers;

public class FileHelper
{
    /// <summary>
    /// Creates directory for <paramref name="destinationFileName"/> if it doesn't exist and copies <paramref name="sourceFileName"/> to it
    /// </summary>
    /// <param name="sourceFileName">Full path of file to copy</param>
    /// <param name="destinationFileName">Full path where to copy the file</param>
    /// <param name="overwrite">True if existing file can be overwritten, otherwise false</param>
    public static void CopyWithDirectoryCreation(string sourceFileName, string destinationFileName, bool overwrite = false)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationFileName)!);
        File.Copy(sourceFileName, destinationFileName, overwrite);
    }
}