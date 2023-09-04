using FluentAssertions;
using KnitterNotebook.Converters;
using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebookTests.Converters
{
    public class CreateYarnDtoConverterTests
    {
        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { "yarn1, yarn2, yarn3", ',', new List<CreateYarnDto>() { new CreateYarnDto("yarn1"), new CreateYarnDto("yarn2"), new CreateYarnDto("yarn3")} };
            yield return new object[] { "yarn1 , yarn2 , yarn3 ", ',', new List<CreateYarnDto>() { new CreateYarnDto("yarn1"), new CreateYarnDto("yarn2"), new CreateYarnDto("yarn3")} };
            yield return new object[] { " yarn1 ;    yarn2;  yarn3     ", ';', new List<CreateYarnDto>() { new CreateYarnDto("yarn1"), new CreateYarnDto("yarn2"), new CreateYarnDto("yarn3")} };
            yield return new object[] { "yarn1    -    yarn2-yarn3", '-', new List<CreateYarnDto>() { new CreateYarnDto("yarn1"), new CreateYarnDto("yarn2"), new CreateYarnDto("yarn3")} };
        }

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
}
