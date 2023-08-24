using FluentValidation;
using FluentValidation.TestHelper;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;

namespace KnitterNotebookTests.Validators
{
    public class LogInDtoValidatorTests
    {
        private readonly IValidator<LogInDto> _validator;

        public LogInDtoValidatorTests()
        {
            _validator = new LogInDtoValidator();
        }

        [Fact]
        public void Validate_ForValidData_PassValidation()
        {
            //Arrange
            LogInDto logInDto = new("helloemail@email.com", "ItIsMyPassword123");

            //Act
            TestValidationResult<LogInDto> validationResult = _validator.TestValidate(logInDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null!)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("helloemail.com")]
        [InlineData("helloemail")]
        [InlineData("helloemail.")]
        [InlineData("helloemailcom")]
        [InlineData("helloemailcom@")]
        public void Validate_ForInvalidEmail_FailValidation(string email)
        {
            //Arrange
            LogInDto logInDto = new(email, "ItIsMyPassword123");

            //Act
            TestValidationResult<LogInDto> validationResult = _validator.TestValidate(logInDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null!)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1234567")]
        [InlineData("test")]
        public void Validate_ForInvalidPassword_FailValidation(string password)
        {
            //Arrange
            LogInDto logInDto = new("helloemail@email.com", password);

            //Act
            TestValidationResult<LogInDto> validationResult = _validator.TestValidate(logInDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}