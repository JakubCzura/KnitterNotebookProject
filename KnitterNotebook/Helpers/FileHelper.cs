using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public class FileHelper
    {
        public static void CopyWithDirectoryCreation(string sourceFileName, string destinationFileName)
        {
            new FileInfo(destinationFileName)?.Directory?.Create();
            File.Copy(sourceFileName, destinationFileName);
        }

        public static void DeleteUnusedUserImages(IEnumerable<Sample> userSamples, string nickname)
        {
            if (Directory.Exists(Paths.UserDirectory(nickname)))
            {
                IEnumerable<string?> userImagesPath = userSamples.Where(x => x.Image != null).Select(x => x?.Image?.Path);
                IEnumerable<string?> imagesToDelete = Directory.GetFiles(Paths.UserDirectory(nickname)).Except(userImagesPath);
                foreach (var image in imagesToDelete)
                {
                    if (image != null)
                    {
                        File.Delete(image);
                    }
                }
            }
        }
    }
}