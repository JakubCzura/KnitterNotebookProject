using FluentValidation.TestHelper;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Validators;

namespace KnitterNotebook.UnitTests.Validators;

public class CreateNeedleDtoValidatorTests
{
    private readonly CreateNeedleDtoValidator _validator = new();

    [Theory]
    [InlineData(0.2, NeedleSizeUnit.mm)]
    [InlineData(60, NeedleSizeUnit.cm)]
    public void Validate_ForValidData_PassValidation(double size, NeedleSizeUnit sizeUnit)
    {
        //Arrange
        CreateNeedleDto createNeedleDto = new(size, sizeUnit);

        //Act
        TestValidationResult<CreateNeedleDto> validationResult = _validator.TestValidate(createNeedleDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-1.2)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(100.1)]
    [InlineData(260)]
    public void Validate_ForInvalidSize_FailValidation(double size)
    {
        //Arrange
        CreateNeedleDto createNeedleDto = new(size, NeedleSizeUnit.cm);

        //Act
        TestValidationResult<CreateNeedleDto> validationResult = _validator.TestValidate(createNeedleDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Size);
    }

    [Fact]
    public void ValidateForInvalidSizeUnit_FailValidation()
    {
        //Arange
        CreateNeedleDto createNeedleDto = new(1, (NeedleSizeUnit)6);

        //Act
        TestValidationResult<CreateNeedleDto> validationResult = _validator.TestValidate(createNeedleDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.SizeUnit);
    }
}