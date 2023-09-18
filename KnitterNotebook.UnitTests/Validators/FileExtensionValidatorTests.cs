using FluentAssertions;
using KnitterNotebook.Validators;

namespace KnitterNotebook.UnitTests.Validators;

public class FileExtensionValidatorTests
{
    [Theory]
    [InlineData(@"C:\Users\computer\Downloads\Image1.jpg")]
    [InlineData(@"C:\Users\Image1.jPG")]
    [InlineData(@"C:\Users\computer\Downloads\Image1.JpG")]
    [InlineData(@"C:\Users\computer\Downloads\Image1.JPg")]
    [InlineData(@"C:\computer\Image2.jpeg")]
    [InlineData(@"C:\computer\Files\Image2.png")]
    public void IsImage_ForValidFilePath_PassValidation(string filePath)
    {
        //Act
        bool result = FileExtensionValidator.IsImage(filePath);

        //Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(@"C:\Computer\Files\Image1.mp3\")]
    [InlineData(@"C:\Computer\Files2\Imag2.mp4")]
    [InlineData(@"C:\Files2\Image3.")]
    [InlineData(@"C:\Computer\Files2\Image4")]
    [InlineData(@"C:\Computer\Image4. jpg")]
    [InlineData(@"C:\Files2\Image4. pdf")]
    [InlineData(@"C:\Computer\Files.")]
    public void IsImage_ForInvalidFilePath_FailValidation(string filePath)
    {
        //Act
        bool result = FileExtensionValidator.IsImage(filePath);

        //Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(@"C:\Users\computer\Downloads\PdfFile.pdf")]
    [InlineData(@"C:\Users\computer\Downloads\FileName.pDF")]
    [InlineData(@"C:\Users\PdfFile.PdF")]
    [InlineData(@"C:\Users\Downloads\File2.Pdf")]
    [InlineData(@"C:\Users\computer\Downloads\File1.pdf")]
    public void IsPdf_ForValidData_PassValidation(string filePath)
    {
        //Act
        bool result = FileExtensionValidator.IsPdf(filePath);

        //Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(@"C:\Desktop\MyDirectory\Image1.mp3\")]
    [InlineData(@"C:\MyDirectory\Imag2.mp4")]
    [InlineData(@"C:\Desktop\MyDirectory\Image3.")]
    [InlineData(@"C:\Desktop\Image4")]
    [InlineData(@"C:\Desktop\FiMyDirectoryles2\Image4. jpg")]
    [InlineData(@"C:\MyDirectory\Image4. pdf")]
    [InlineData(@"C:\Desktop\MyDirectory.")]
    public void IsPdf_ForInvalidData_FailValidation(string filePath)
    {
        //Act
        bool result = FileExtensionValidator.IsPdf(filePath);

        //Assert
        result.Should().BeFalse();
    }
}