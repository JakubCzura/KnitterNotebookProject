using FluentAssertions;
using KnitterNotebook.Converters.XAMLConverters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Globalization;
using System.Windows;

namespace KnitterNotebook.UnitTests.Converters.XAMLConverters;

public class NeedleDtoToStringConverterTests
{
    [Fact]
    public void Convert_ForValidData_ReturnsConvertedString()
    {
        //Arrange
        NeedleDtosToStringConverter converter = new();
        List<NeedleDto> needleDtos = new()
        {
            new NeedleDto(new Needle(15, NeedleSizeUnit.cm )),
            new NeedleDto(new Needle(25, NeedleSizeUnit.mm)),
            new NeedleDto(new Needle(3, NeedleSizeUnit.cm )),
        };
        string expected = "15 cm\n25 mm\n3 cm";

        //Act
        string result = (string)converter.Convert(needleDtos, typeof(string), null!, CultureInfo.CurrentCulture);

        //Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Convert_ForInvalidData_ReturnsEmptyString()
    {
        //Arrange
        NeedleDtosToStringConverter converter = new();
        List<NeedleDto> needleDtos = null!;

        //Act
        string result = (string)converter.Convert(needleDtos, typeof(string), null!, CultureInfo.CurrentCulture);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact()]
    public void ConvertBack_ReturnsUnsetValue()
    {
        //Arrange
        NeedleDtosToStringConverter converter = new();

        //Act
        var result = converter.ConvertBack("15 cm\n25 mm\n3 cm", typeof(IEnumerable<NeedleDto>), null!, CultureInfo.CurrentCulture);

        //Assert
        result.Should().Be(DependencyProperty.UnsetValue);
    }
}