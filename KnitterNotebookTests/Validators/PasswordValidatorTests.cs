using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Throw;
using FluentAssertions;

namespace KnitterNotebookTests.Validators
{
    public class PasswordValidatorTests
    {
        private readonly PasswordValidator _validator;

        public PasswordValidatorTests()
        {
            _validator = new PasswordValidator();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { string.Empty };
            yield return new object[] { "" };
            yield return new object[] { "invaliddpassword" };
            yield return new object[] { "invaliddpassword1" };
            yield return new object[] { "invaliddpassword2" };
            yield return new object[] { " a "};
            yield return new object[] { " " };
            yield return new object[] { "                    " };
            yield return new object[] { "invalidPassword" };
            yield return new object[] { "Testpassword" };
            yield return new object[] { "." };
            yield return new object[] { ".                                             " };
            yield return new object[] { "p@m1" };
            yield return new object[] { "p@ m1" };
            yield return new object[] { new string('K', 51) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { "ValidPassword1" };
            yield return new object[] { "ValidPassword1@" };
            yield return new object[] { "PasswordAccepted123@" };
            yield return new object[] { "P123123@k" };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void Validate_ForInvalidData_FailValidation(string password)
        {
            //Act
            var validationResult = _validator.TestValidate(password);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Validate_ForNullData_FailValidation()
        {
            string password = null!;

            //Act
            Action action = () => _validator.TestValidate(password);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void Validate_ForValidData_PassValidation(string password)
        {
            //Act
            var validationResult = _validator.TestValidate(password);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}
