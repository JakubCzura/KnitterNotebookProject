using FluentAssertions;
using FluentValidation.TestHelper;
using KnitterNotebook.Validators;

namespace KnitterNotebook.UnitTests.Validators;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _validator = new();

    [Theory]
    [InlineData("ValidPassword1")]
    [InlineData("ValidPassword1@")]
    [InlineData("PasswordAccepted123@")]
    [InlineData("P123123@k")]
    public void Validate_ForValidPassword_PassValidation(string password)
    {
        //Act
        TestValidationResult<string> validationResult = _validator.TestValidate(password);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    public static TheoryData<string> InvalidData => new()
    {
        { string.Empty },
        { "invaliddpassword" },
        { "invaliddpassword1" },
        { "invaliddpassword2" },
        { " a " },
        { " " },
        { new string(' ', 20) },
        { "invalidPassword" },
        { "Testpassword" },
        { "." },
        { "." + new string(' ', 50) },
        { "p@m1" },
        { "p@ m1" },
        { new string('K', 51) }
    };

    [Theory]
    [MemberData(nameof(InvalidData))]
    public void Validate_ForInvalidPassword_FailValidation(string password)
    {
        //Act
        TestValidationResult<string> validationResult = _validator.TestValidate(password);

        //Assert
        validationResult.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_ForNullData_FailValidation()
    {
        //Arrange
        string password = null!;

        //Act
        Action action = () => _validator.TestValidate(password);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }
}