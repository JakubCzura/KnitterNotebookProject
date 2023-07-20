using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KnitterNotebookTests.HelpersForTesting;

namespace KnitterNotebookTests.Validators
{
    public class RegisterUserDtoValidatorTests
    {
        private readonly RegisterUserDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;

        public RegisterUserDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new RegisterUserDtoValidator(_databaseContext);
            SeedUsers();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new RegisterUserDto(null!, "valid@email.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("", "valid@email.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto(new string('K', 51), "valid@email.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto(new string('K', 51), "valid@email.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Nick1", "valid@email.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Nick2", "valid@email.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", null!, "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "nick1@mail.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "nick2@mail.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", new string('K', 51) + "@mail.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "mail.com", "ValidPassword123@#") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "validemail@mail.com", "") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "validemail@mail.com", null!) };
            yield return new object[] { new RegisterUserDto("Valid Nick", "validemail@mail.com", new string('K',5)) };
            yield return new object[] { new RegisterUserDto("Valid Nick", "validemail@mail.com", "kkdsgkkK") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "validemail@mail.com", "KKDAS75645") };
            yield return new object[] { new RegisterUserDto("Valid Nick", "validemail@mail.com", "6542345") };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new RegisterUserDto("ValidNickname", "uservalidemail@mail.com", "Pass123@word") };
            yield return new object[] { new RegisterUserDto("Nickname", "emailuser@mail.com", "Strong321@xSd") };
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1, Email = "nick1@mail.com", Nickname = "Nick1"},
                new User() { Id = 2, Email = "nick2@mail.com", Nickname = "Nick2"},
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ValidateAsync_ForInvalidData_FailValidation(RegisterUserDto registerUserDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(registerUserDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(RegisterUserDto registerUserDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(registerUserDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}
