using System.IO;

namespace KnitterNotebook.Helpers
{
    public class FileHelper
    {
        public static void CopyWithDirectoryCreation(string sourceFileName, string destinationFileName)
        {
            new FileInfo(destinationFileName)?.Directory?.Create();
            File.Copy(sourceFileName, destinationFileName);
        }
    }
}