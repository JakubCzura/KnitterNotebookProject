using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Helpers
{
    public class FileHelper
    {
        public static bool CopyFileWithDirectoryCreation(string fileToSave, string newFile)
        {
            new FileInfo(newFile)?.Directory?.Create();
            if (File.Exists(newFile))
            {
                return false;
            }
            else
            {
                File.Copy(fileToSave, newFile);
                return true;
            }
        }
    }
}
