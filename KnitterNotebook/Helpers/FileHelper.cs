using System.IO;

namespace KnitterNotebook.Helpers
{
    public class FileHelper
    {
        public static void CopyFileWithDirectoryCreation(string fileToSave, string newFile)
        {
            new FileInfo(newFile)?.Directory?.Create();
            File.Copy(fileToSave, newFile);
        }
    }
}