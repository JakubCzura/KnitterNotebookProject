using FluentAssertions;
using KnitterNotebook.Converters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;

namespace KnitterNotebook.UnitTests.Converters;

public class YarnsNamesWithDelimiterConverterTests
{
    [Fact]
    public void Convert_ForGivenYarnsAndDefaultParameter_ReturnsYarnsNamesAsString()
    {
        //Arrange
        List<YarnDto> yarns = new()
        {
            new (new Yarn("Yarn1")),
            new (new Yarn( "Merino")),
            new (new Yarn("Super Yarn"))
        };
        string expected = "Yarn1,Merino,Super Yarn";

        //Act
        string result = YarnsNamesWithDelimiterConverter.Convert(yarns);

        //Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Convert_ForGivenYarnsAndCustomParameter_ReturnsYarnsNamesAsString()
    {
        //Arrange
        List<YarnDto> yarns = new()
        {
            new (new Yarn("Yarn1")),
            new (new Yarn("Merino")),
            new (new Yarn("Super Yarn"))
        };
        string expected = "Yarn1;Merino;Super Yarn";
        char delimiter = ';';

        //Act
        string result = YarnsNamesWithDelimiterConverter.Convert(yarns, delimiter);

        //Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Convert_ForEmptyYarns_ReturnsEmptyString()
    {
        //Arrange
        List<YarnDto> yarns = new();

        //Act
        string result = YarnsNamesWithDelimiterConverter.Convert(yarns);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Convert_ForNullYarns_ThrowArgumentNullException()
    {
        //Arrange
        List<YarnDto> yarns = null!;

        //Act
        Action act = () => YarnsNamesWithDelimiterConverter.Convert(yarns);

        //Assert
        act.Should().Throw<ArgumentNullException>();
    }
}