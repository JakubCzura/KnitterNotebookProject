using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class ChangeEmailDtoValidatorTests
    {
        private readonly ChangeEmailDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _paswordServiceMock = new();
        public ChangeEmailDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _paswordServiceMock.Object);
            _validator = new ChangeEmailDtoValidator(_userService);
            SeedUsers();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new ChangeEmailDto(0, null!) };
            yield return new object[] { new ChangeEmailDto(-1, string.Empty) };
            yield return new object[] { new ChangeEmailDto(2, string.Empty) };
            yield return new object[] { new ChangeEmailDto(3, string.Empty) };
            yield return new object[] { new ChangeEmailDto(4, new string('K', 51) + "@mail.com") };
            yield return new object[] { new ChangeEmailDto(5, new string('K', 60) + "@testmail.") };
            yield return new object[] { new ChangeEmailDto(1, "test1@test.com") };
            yield return new object[] { new ChangeEmailDto(2, "test2@test.com") };
            yield return new object[] { new ChangeEmailDto(3, "test3@test.com") };
            yield return new object[] { new ChangeEmailDto(4, "test3@test.com") };
            yield return new object[] { new ChangeEmailDto(3, "@@") };
            yield return new object[] { new ChangeEmailDto(3, "...") };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeEmailDto(1, "newemail@test.com") };
            yield return new object[] { new ChangeEmailDto(2, "newemail@test.com") };
            yield return new object[] { new ChangeEmailDto(3, "newemail@test.com") };
            yield return new object[] { new ChangeEmailDto(1, "testemailnew@test.com") };
            yield return new object[] { new ChangeEmailDto(2, "newtest@testnew.com") };
            yield return new object[] { new ChangeEmailDto(3, "newemail@testemailnew.com") };
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1, Email = "test1@test.com"},
                new User() { Id = 2, Email = "test2@test.com"},
                new User() { Id = 3, Email = "test3@test.com"}
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ValidateAsync_ForInvalidData_FailValidation(ChangeEmailDto changeEmailDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changeEmailDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangeEmailDto changeEmailDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changeEmailDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}