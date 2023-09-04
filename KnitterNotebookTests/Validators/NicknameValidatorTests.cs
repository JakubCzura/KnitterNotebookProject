using FluentAssertions;
using FluentValidation.TestHelper;
using KnitterNotebook.Validators;

namespace KnitterNotebookTests.Validators
{
    public class NicknameValidatorTests
    {
        private readonly NicknameValidator _validator;

        public NicknameValidatorTests()
        {
            _validator = new NicknameValidator();
        }

        public static IEnumerable<object[]> InvalidNicknames()
        {
            yield return new object[] { string.Empty };
            yield return new object[] { "" };
            yield return new object[] { new string(' ', 40) };
            yield return new object[] { new string(' ', 51) };
            yield return new object[] { " " };
            yield return new object[] { "invalid!" };
            yield return new object[] { "invalid1@" };
            yield return new object[] { "." };
            yield return new object[] { "p@m1" };
            yield return new object[] { "p@ m1" };
            yield return new object[] { new string('K', 51) };
        }

        [Theory]
        [MemberData(nameof(InvalidNicknames))]
        public void Validate_ForInvalidNickname_FailValidation(string nickname)
        {
            //Act
            TestValidationResult<string> validationResult = _validator.TestValidate(nickname);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Validate_ForNullNickname_ThrowArgumentNullException()
        {
            string nickname = null!;

            //Act
            Action action = () => _validator.TestValidate(nickname);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Nickname1")]
        [InlineData("1Name1")]
        [InlineData("13212MyName")]
        [InlineData("122141")]
        [InlineData("My nickname")]
        [InlineData("My humorous nickname")]
        public void Validate_ForValidNickname_PassValidation(string nickname)
        {
            //Act
            TestValidationResult<string> validationResult = _validator.TestValidate(nickname);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}