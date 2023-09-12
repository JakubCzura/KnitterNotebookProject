using System.IO;

namespace KnitterNotebook.Helpers
{
    public class FileHelper
    {
        public static void CopyWithDirectoryCreation(string sourceFileName, string destinationFileName, bool overwrite = false)
        {
            Directory.CreateDirectory(destinationFileName);
            File.Copy(sourceFileName, destinationFileName, overwrite);
        }
    }
}