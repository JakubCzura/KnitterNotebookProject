using FluentAssertions;
using KnitterNotebook.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebookTests.Validators
{
    public class ImageExtensionValidatorTests
    {
        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { "C:\\Computer\\Files\\Image1.jpg" };
            yield return new object[] { "C:\\Computer\\Files\\Image2.jpeg" };
            yield return new object[] { "C:\\Computer\\Files\\Image3.png" };
            yield return new object[] { "C:\\Computer\\Files\\Image4.gif" };
            yield return new object[] { "C:\\Computer\\Files\\Image5.bmp" };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public static void IsImage_ForValidData_PassValidation(string filePath)
        {
            //Act
            bool result = ImageExtensionValidator.IsImage(filePath);

            //Assert
            result.Should().BeTrue();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { "C:\\Computer\\Files\\Image1.mp3" };
            yield return new object[] { "C:\\Computer\\Files\\Image2.mp4" };
            yield return new object[] { "C:\\Computer\\Files\\Image3." };
            yield return new object[] { "C:\\Computer\\Files\\Image4" };
            yield return new object[] { string.Empty };
            yield return new object[] { null! };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public static void IsImage_ForInvalidData_FailValidation(string filePath)
        {
            //Act
            bool result = ImageExtensionValidator.IsImage(filePath);

            //Assert
            result.Should().BeFalse();
        }
    }
}
