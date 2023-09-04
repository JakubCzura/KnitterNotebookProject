using FluentAssertions;
using KnitterNotebook.Converters.XAMLConverters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Globalization;
using System.Windows;

namespace KnitterNotebookTests.Converters.XAMLConverters
{
    public class YarnDtosToStringConverterTests
    {
        [Fact]
        public void Convert_ForValidData_ReturnsConvertedString()
        {
            //Arrange
            YarnDtosToStringConverter converter = new();
            List<YarnDto> yarnDtos = new()
            {
                new YarnDto(new Yarn() {Name = "yarn's name"} ),
                new YarnDto(new Yarn() {Name = "cotton yarn's name"} ),
                new YarnDto(new Yarn() {Name = "woolen yarn's name"} )
            };
            string expected = "yarn's name\ncotton yarn's name\nwoolen yarn's name";

            //Act
            string result = (string)converter.Convert(yarnDtos, typeof(string), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Convert_ForInvalidData_ReturnsEmptyString()
        {
            //Arrange
            YarnDtosToStringConverter converter = new();
            List<YarnDto> sampleDto = null!;

            //Act
            string result = (string)converter.Convert(sampleDto, typeof(string), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ConvertBack_ReturnsUnsetValue()
        {
            //Arrange
            YarnDtosToStringConverter converter = new();

            //Act
            object result = converter.ConvertBack("yarn's name\nsecond yarn's name", typeof(IEnumerable<YarnDto>), null!, CultureInfo.CurrentCulture);

            //Assert
            result.Should().Be(DependencyProperty.UnsetValue);
        }
    }
}