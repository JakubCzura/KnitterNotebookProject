using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebookTests.Validators
{
    public class ImagePathCreatorTests
    {
        public static IEnumerable<(string, string)> ValidData()
        {
            yield return ("UserName", @"c:\test\test\directory\ImageName.jpg");
        }

        [Fact]
        public void CreateUniquePathToSaveImage_ForValidData_CreatesValidPath()
        {

        }
    }
}
