using FluentValidation.TestHelper;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;

namespace KnitterNotebookTests.Validators
{
    public class CreateYarnDtoValidatorTests
    {
        private readonly CreateYarnDtoValidator _validator;

        public CreateYarnDtoValidatorTests()
        {
            _validator = new CreateYarnDtoValidator();
        }

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

        public static IEnumerable<object[]> InvalidNames()
        {
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { null! };
            yield return new object[] { new string('K', 101) };
        }

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
}