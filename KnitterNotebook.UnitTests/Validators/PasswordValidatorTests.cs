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

    public static IEnumerable<object[]> InvalidData()
    {
        yield return new object[] { string.Empty };
        yield return new object[] { "invaliddpassword" };
        yield return new object[] { "invaliddpassword1" };
        yield return new object[] { "invaliddpassword2" };
        yield return new object[] { " a " };
        yield return new object[] { " " };
        yield return new object[] { new string(' ', 20) };
        yield return new object[] { "invalidPassword" };
        yield return new object[] { "Testpassword" };
        yield return new object[] { "." };
        yield return new object[] { "." + new string(' ', 50) };
        yield return new object[] { "p@m1" };
        yield return new object[] { "p@ m1" };
        yield return new object[] { new string('K', 51) };
    }

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