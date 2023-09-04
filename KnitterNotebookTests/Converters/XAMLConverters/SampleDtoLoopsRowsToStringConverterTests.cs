using FluentAssertions;
using KnitterNotebook.Converters.XAMLConverters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Globalization;
using System.Windows;

namespace KnitterNotebookTests.Converters.XAMLConverters
{
    public class SampleDtoLoopsRowsToStringConverterTests
    {
        [Fact]
        public void Convert_ForValidData_ReturnsConvertedString()
        {
            //Arrange
            SampleDtoLoopsRowsToStringConverter converter = new();
            BasicSampleDto sampleDto = new(new Sample { LoopsQuantity = 5, RowsQuantity = 6 });
            string expected = "5x6";

            //Act
            string result = (string)converter.Convert(sampleDto, typeof(string), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Convert_ForInvalidData_ReturnsEmptyString()
        {
            //Arrange
            SampleDtoLoopsRowsToStringConverter converter = new();
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
            SampleDtoLoopsRowsToStringConverter converter = new();

            //Act
            object result = converter.ConvertBack("5x5", typeof(BasicSampleDto), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().Be(DependencyProperty.UnsetValue);
        }
    }
}