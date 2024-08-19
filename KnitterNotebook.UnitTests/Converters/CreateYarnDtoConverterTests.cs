using FluentAssertions;
using KnitterNotebook.Converters;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.UnitTests.Converters;

public class CreateYarnDtoConverterTests
{
    public static TheoryData<string, char, List<CreateYarnDto>> ValidData => new()
    {
        { "yarn1, yarn2, yarn3", ',', new List<CreateYarnDto>() { new("yarn1"), new("yarn2"), new("yarn3") } },
        { "yarn1 , yarn2 , yarn3 ", ',', new List<CreateYarnDto>() { new("yarn1"), new("yarn2"), new("yarn3") } },
        { " yarn1 ;    yarn2;  yarn3     ", ';', new List<CreateYarnDto>() { new("yarn1"), new("yarn2"), new("yarn3") } },
        { "yarn1    -    yarn2-yarn3", '-', new List<CreateYarnDto>() { new("yarn1"), new("yarn2"), new("yarn3") } }
    };

    [Theory]
    [MemberData(nameof(ValidData))]
    public void Convert_ValidData_ReturnsListOfCreateYarnDto(string yarnsNamesWithDelimiter, char delimiter, List<CreateYarnDto> expected)
    {
        //Act
        IEnumerable<CreateYarnDto> result = CreateYarnDtoConverter.Convert(yarnsNamesWithDelimiter, delimiter);

        //Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Convert_ForInvalidData_ReturnsEmptyIEnumerable(string yarnsNamesWithDelimiter)
    {
        //Arrange
        char delimiter = ',';

        //Act
        IEnumerable<CreateYarnDto> result = CreateYarnDtoConverter.Convert(yarnsNamesWithDelimiter, delimiter);

        //Assert
        result.Should().BeEmpty();
    }
}