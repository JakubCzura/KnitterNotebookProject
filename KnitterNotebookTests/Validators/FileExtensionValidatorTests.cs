using FluentAssertions;
using KnitterNotebook.Validators;

namespace KnitterNotebookTests.Validators
{
    public class FileExtensionValidatorTests
    {
        public static IEnumerable<object[]> ImageValidData()
        {
            yield return new object[] { @"C:\Users\computer\Downloads\Image1.jpg" };
            yield return new object[] { @"C:\Users\computer\Downloads\Image1.jPg" };
            yield return new object[] { @"C:\Users\computer\Downloads\Image1.jPG" };
            yield return new object[] { @"C:\Users\computer\Downloads\Image1.JpG" };
            yield return new object[] { @"C:\Users\computer\Downloads\Image1.JPg" };
            yield return new object[] { @"C:\computer\Files\Image2.jpeg" };
            yield return new object[] { @"C:\computer\Files\Image3.png" };
        }

        public static IEnumerable<object[]> PdfValidData()
        {
            yield return new object[] { @"C:\Users\computer\Downloads\File.pdf" };
            yield return new object[] { @"C:\Users\computer\Downloads\File.pDf" };
            yield return new object[] { @"C:\Users\computer\Downloads\File.pDF" };
            yield return new object[] { @"C:\Users\computer\Downloads\File.PdF" };
            yield return new object[] { @"C:\Users\computer\Downloads\File.Pdf" };
            yield return new object[] { @"C:\computer\Files\File1.pdf" };
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { @"C:\Computer\Files\Image1.mp3" };
            yield return new object[] { @"C:\Computer\Files\Image2.mp4" };
            yield return new object[] { @"C:\Computer\Files\Image3." };
            yield return new object[] { @"C:\Computer\Files\Image4" };
            yield return new object[] { @"C:\Computer\Files\Image4. jpg" };
            yield return new object[] { @"C:\Computer\Files\Image4. pdf" };
            yield return new object[] { @"C:\Computer\Files." };
            yield return new object[] { string.Empty };
            yield return new object[] { null! };
        }

        [Theory]
        [MemberData(nameof(ImageValidData))]
        public void IsImage_ForValidData_PassValidation(string filePath)
        {
            //Act
            bool result = FileExtensionValidator.IsImage(filePath);

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void IsImage_ForInvalidData_FailValidation(string filePath)
        {
            //Act
            bool result = FileExtensionValidator.IsImage(filePath);

            //Assert
            result.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(PdfValidData))]
        public void IsPdf_ForValidData_PassValidation(string filePath)
        {
            //Act
            bool result = FileExtensionValidator.IsPdf(filePath);

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void IsPdf_ForInvalidData_FailValidation(string filePath)
        {
            //Act
            bool result = FileExtensionValidator.IsPdf(filePath);

            //Assert
            result.Should().BeFalse();
        }
    }
}