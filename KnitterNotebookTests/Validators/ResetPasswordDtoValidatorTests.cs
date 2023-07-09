using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebookTests.Validators
{
    public class ResetPasswordDtoValidatorTests
    {
        private readonly ResetPasswordDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;

        public ResetPasswordDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new ResetPasswordDtoValidator(_databaseContext);
            SeedUsers();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new ResetPasswordDto("test@test.com", "PasswordNew123", "PasswordNew123")};
            yield return new object[] { new ResetPasswordDto("Nick333", "PasswordNew123", "PasswordNew123")};
            yield return new object[] { new ResetPasswordDto("nick1@mail.com", "KPasswordNew123", "PasswordNew123")};
            yield return new object[] { new ResetPasswordDto("Nick1@mail.com", "KPass4wordNew123", "PasswordNew123")};
            yield return new object[] { new ResetPasswordDto("nick2@mail.com", "KP3assw5ordNew123", "PasswordNew123")};
            yield return new object[] { new ResetPasswordDto("Nick2@mail.com", "KPa3ssw56ordNew123", "PasswordNew123")};
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ResetPasswordDto("nick1@mail.com", "PasswordNew123", "PasswordNew123") };
            yield return new object[] { new ResetPasswordDto("nick2@mail.com", "PasswordNew123", "PasswordNew123") };
            yield return new object[] { new ResetPasswordDto("Nick1", "PasswordNew123", "PasswordNew123") };
            yield return new object[] { new ResetPasswordDto("Nick2", "PasswordNew123", "PasswordNew123") };
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
        public async Task ValidateAsync_ForInvalidData_FailValidation(ResetPasswordDto resetPasswordDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ResetPasswordDto resetPasswordDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}