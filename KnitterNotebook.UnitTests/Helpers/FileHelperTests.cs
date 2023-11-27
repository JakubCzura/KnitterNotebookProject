using FluentAssertions;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Helpers;

namespace KnitterNotebook.UnitTests.Helpers;

public class FileHelperTests
{
    [Fact]
    public void CopyWithDirectoryCreation_ForNullSourceFileName_ThrowsArgumentNullException()
    {
        // Arrange
        string sourceFileName = null!;
        string destinationFileName = @"c:\computer\files\file.txt";

        // Act
        Action action = () => FileHelper.CopyWithDirectoryCreation(sourceFileName, destinationFileName);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CopyWithDirectoryCreation_ForNullDestinationFileName_ThrowsArgumentNullException()
    {
        //Arrange
        string sourceFileName = @"c:\computer\files\file.txt";
        string destinationFileName = null!;

        //Act
        Action action = () => FileHelper.CopyWithDirectoryCreation(sourceFileName, destinationFileName);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CopyWithDirectoryCreation_ForNotExistingFileToCopy_ThrowsFileNotFoundException()
    {
        //Arrange
        //Situation when source file doesn't exist in specified existing directory
        string sourceFileName = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "FileThatDoesntExist.txt");
        string destinationFileName = @"c:\computer\files\file.txt";

        //Act
        Action action = () => FileHelper.CopyWithDirectoryCreation(sourceFileName, destinationFileName);

        //Assert
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void CopyWithDirectoryCreation_ForNotExistingDirectoryOfFileToCopy_ThrowsDirectoryNotFoundException()
    {
        //Arrange
        //Situation when source file's path consists directory that doesn't exist
        string sourceFileName = @"c:\c\c\c\c\c\c\c\c\c\files\file.txt";
        string destinationFileName = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "NewFile.txt");

        //Act
        Action action = () => FileHelper.CopyWithDirectoryCreation(sourceFileName, destinationFileName);

        //Assert
        action.Should().Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void CopyWithDirectoryCreation_ForSelectedFile_CopiesFile()
    {
        //Arrange
        string sourceFileName = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "ProjectImage.jpg");
        string destinationFileName = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "NewFile.jpg");

        //Act
        FileHelper.CopyWithDirectoryCreation(sourceFileName, destinationFileName);

        //Assert
        File.Exists(destinationFileName).Should().BeTrue();

        //Clean up after test
        File.Delete(destinationFileName);
    }

    [Fact]
    public void CopyWithDirectoryCreation_ForSelectedFileAndLackOfDestinationFileNecessaryDirectory_CopiesFileAndCreatesNecessaryDirectory()
    {
        //Arrange
        string sourceFileName = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "ProjectImage.jpg");
        string destinationFileName = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "NextDirectory", "NextDirectory", "NewFile.jpg");

        //Act
        FileHelper.CopyWithDirectoryCreation(sourceFileName, destinationFileName);

        //Assert
        File.Exists(destinationFileName).Should().BeTrue();

        //Clean up after test
        File.Delete(destinationFileName);
    }
}