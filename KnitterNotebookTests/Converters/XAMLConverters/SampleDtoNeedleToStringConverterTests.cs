using FluentAssertions;
using KnitterNotebook.Converters.XAMLConverters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Globalization;
using System.Windows;

namespace KnitterNotebookTests.Converters.XAMLConverters
{
    public class SampleDtoNeedleToStringConverterTests
    {
        [Fact]
        public void Convert_ForValidData_ReturnsConvertedString()
        {
            //Arrange
            SampleDtoNeedleToStringConverter converter = new();
            BasicSampleDto sampleDto = new(new Sample { NeedleSize = 6, NeedleSizeUnit = NeedleSizeUnit.cm });
            string expected = "6 cm";

            //Act
            string result = (string)converter.Convert(sampleDto, typeof(string), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Convert_ForInvalidData_ReturnsEmptyString()
        {
            //Arrange
            SampleDtoNeedleToStringConverter converter = new();
            BasicSampleDto sampleDto = null!;

            //Act
            string result = (string)converter.Convert(sampleDto, typeof(string), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ConvertBack_ReturnsUnsetValue()
        {
            //Arrange
            SampleDtoNeedleToStringConverter converter = new();

            //Act
            object result = converter.ConvertBack("6cm", typeof(SampleDto), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().Be(DependencyProperty.UnsetValue);
        }
    }
}