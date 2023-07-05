using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebookTests.Validators
{
    public class ChangePasswordDtoValidatorTests
    {
        private readonly ChangePasswordDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;

        public ChangePasswordDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new ChangePasswordDtoValidator(_databaseContext);
            SeedUsers();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new ChangePasswordDto(-1, string.Empty, string.Empty) };
            yield return new object[] { new ChangePasswordDto(0, null!, null!) };
            yield return new object[] { new ChangePasswordDto(0, string.Empty, null!) };
            yield return new object[] { new ChangePasswordDto(0, null!, string.Empty) };
            yield return new object[] { new ChangePasswordDto(1, null!, null!) };
            yield return new object[] { new ChangePasswordDto(2, string.Empty, null!) };
            yield return new object[] { new ChangePasswordDto(3, null!, string.Empty) };
            yield return new object[] { new ChangePasswordDto(4, string.Empty, null!) };
            yield return new object[] { new ChangePasswordDto(5, null!, string.Empty!) };
            yield return new object[] { new ChangePasswordDto(1, "invaliddpassword", "invaliddpassword") };
            yield return new object[] { new ChangePasswordDto(0, "invaliddpassword", "invaliddpassword") };
            yield return new object[] { new ChangePasswordDto(-10, "invaliddpassword", "invaliddpassword") };
            yield return new object[] { new ChangePasswordDto(1, "invaliddpassword1", "invaliddpassword1") };
            yield return new object[] { new ChangePasswordDto(0, "invaliddpassword2", "invaliddpassword2") };
            yield return new object[] { new ChangePasswordDto(-10, "invaliddpassword2", "invaliddpassword2") };
            yield return new object[] { new ChangePasswordDto(1, " a ", "invaliddpassword2") };
            yield return new object[] { new ChangePasswordDto(1, " a ", " a ") };
            yield return new object[] { new ChangePasswordDto(2, "ValidPassword1", "invalidPassword") };
            yield return new object[] { new ChangePasswordDto(2, "Testpassword", "ValidPassword1") };
            yield return new object[] { new ChangePasswordDto(12, "ValidPassword1", "ValidPassword1") };
            yield return new object[] { new ChangePasswordDto(2, ".", "ValidPassword1") };
            yield return new object[] { new ChangePasswordDto(2, ".", ".") };
            yield return new object[] { new ChangePasswordDto(2, "p@m1", "p@m1") };
            yield return new object[] { new ChangePasswordDto(2, "p@m1", "p22@m1") };
            yield return new object[] { new ChangePasswordDto(2, new string ('K', 51), new string('K', 51)) };
            yield return new object[] { new ChangePasswordDto(2, ".", new string('K', 51)) };
            yield return new object[] { new ChangePasswordDto(12, "Ja@d1", new string('K', 51)) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangePasswordDto(2, "ValidPassword1", "ValidPassword1") };
            yield return new object[] { new ChangePasswordDto(2, "ValidPassword1@", "ValidPassword1@") };
            yield return new object[] { new ChangePasswordDto(1, "PasswordAccepted123@", "PasswordAccepted123@") };
            yield return new object[] { new ChangePasswordDto(3, "P123123@k", "P123123@k") };
        }

        private static string CreateUniqueDatabaseName => "TestDb" + DateTime.Now.Ticks.ToString();

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1 },
                new User() { Id = 2 },
                new User() { Id = 3 }
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ValidateAsync_ForInvalidData_FailValidation(ChangePasswordDto changePasswordDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changePasswordDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangePasswordDto changePasswordDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changePasswordDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}