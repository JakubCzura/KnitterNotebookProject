using FluentAssertions;
using KnitterNotebook.Converters;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.UnitTests.Converters
{
    public class CreateNeedleDtoConverterTests
    {
        [Fact]
        public void Convert_ForValidData_ReturnsCreateNeedleDto()
        {
            //Arrange
            NullableSizeNeedle nullableSizeNeedle = new()
            {
                Size = 3.5,
                SizeUnit = Models.Enums.NeedleSizeUnit.mm
            };

            //Act
            CreateNeedleDto createNeedleDto = CreateNeedleDtoConverter.Convert(nullableSizeNeedle);

            //Assert
            createNeedleDto.Size.Should().Be(nullableSizeNeedle.Size);
            createNeedleDto.SizeUnit.Should().Be(nullableSizeNeedle.SizeUnit);
        }

        [Fact]
        public void Convert_ForNullData_ThrowsNullReferenceException()
        {
            //Arrange
            NullableSizeNeedle nullableSizeNeedle = null!;

            //Act
            Action act = () => CreateNeedleDtoConverter.Convert(nullableSizeNeedle);

            //Assert
            act.Should().Throw<NullReferenceException>();
        }
    }
}