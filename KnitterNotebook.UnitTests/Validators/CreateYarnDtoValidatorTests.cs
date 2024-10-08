﻿using FluentValidation.TestHelper;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;

namespace KnitterNotebook.UnitTests.Validators;

public class CreateYarnDtoValidatorTests
{
    private readonly CreateYarnDtoValidator _validator = new();

    [Theory]
    [InlineData("Yarn name")]
    [InlineData("K")]
    [InlineData("Sample woolen yarn name")]
    public void Validate_ForValidData_PassValidation(string name)
    {
        //Arrange
        CreateYarnDto createYarnDto = new(name);

        //Act
        TestValidationResult<CreateYarnDto> validationResult = _validator.TestValidate(createYarnDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    public static TheoryData<string> InvalidNames => new()
    {
        { "" },
        { " " },
        { null! },
        { new string('K', 101) }
    };

    [Theory]
    [MemberData(nameof(InvalidNames))]
    public void Validate_ForInvalidName_FailValidation(string name)
    {
        //Arrange
        CreateYarnDto createYarnDto = new(name);

        //Act
        TestValidationResult<CreateYarnDto> validationResult = _validator.TestValidate(createYarnDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
    }
}