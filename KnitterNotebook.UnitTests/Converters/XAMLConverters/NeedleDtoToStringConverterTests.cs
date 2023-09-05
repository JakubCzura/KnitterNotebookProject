using FluentAssertions;
using KnitterNotebook.Converters.XAMLConverters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Globalization;
using System.Windows;

namespace KnitterNotebook.UnitTests.Converters.XAMLConverters
{
    public class NeedleDtoToStringConverterTests
    {
        [Fact]
        public void Convert_ForValidData_ReturnsConvertedString()
        {
            //Arrange
            NeedleDtosToStringConverter converter = new();
            List<NeedleDto> needleDtos = new()
            {
                new NeedleDto(new Needle { Size = 15, SizeUnit = NeedleSizeUnit.cm }),
                new NeedleDto(new Needle { Size = 25, SizeUnit = NeedleSizeUnit.mm }),
                new NeedleDto(new Needle { Size = 3, SizeUnit = NeedleSizeUnit.cm }),
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
}