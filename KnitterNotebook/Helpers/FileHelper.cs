using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
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

        public static void DeleteUnusedUserImages(IEnumerable<SampleDto> userSamples, string nickname)
        {
            if (Directory.Exists(Paths.UserDirectory(nickname)))
            {
                IEnumerable<string?> userImagesPath = userSamples.Where(x => !string.IsNullOrWhiteSpace(x.ImagePath)).Select(x => x.ImagePath);
                IEnumerable<string?> imagesToDelete = Directory.GetFiles(Paths.UserDirectory(nickname)).Where(FileExtensionValidator.IsImage).Except(userImagesPath);
                foreach (string? image in imagesToDelete.Where(image => image is not null))
                {
                    if (File.Exists(image))
                    {
                        File.Delete(image);
                    }
                }
            }
        }
    }
}