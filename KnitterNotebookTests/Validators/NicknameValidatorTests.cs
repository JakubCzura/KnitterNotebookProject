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

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { string.Empty };
            yield return new object[] { "" };
            yield return new object[] { new string(' ', 40) };
            yield return new object[] { new string(' ', 51) };
            yield return new object[] { " a " };
            yield return new object[] { " " };
            yield return new object[] { "invalid!" };
            yield return new object[] { "invalid1@" };
            yield return new object[] { "." };
            yield return new object[] { "p@m1" };
            yield return new object[] { "p@ m1" };
            yield return new object[] { new string('K', 51) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { "Nickname" };
            yield return new object[] { "Nickname1" };
            yield return new object[] { "1Nickname1" };
            yield return new object[] { "11231Nickname" };
            yield return new object[] { "12221" };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void Validate_ForInvalidData_FailValidation(string nickanem)
        {
            //Act
            var validationResult = _validator.TestValidate(nickanem);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Validate_ForNullData_ThrowArgumentNullException()
        {
            string nickname = null!;

            //Act
            Action action = () => _validator.TestValidate(nickname);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void Validate_ForValidData_PassValidation(string nickname)
        {
            //Act
            var validationResult = _validator.TestValidate(nickname);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}