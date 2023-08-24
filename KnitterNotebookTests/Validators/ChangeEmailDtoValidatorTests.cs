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

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeEmailDto(1, "testemailnew@test.com") };
            yield return new object[] { new ChangeEmailDto(2, "newtest@testnew.com") };
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1, Email = "test1@test.com"},
                new User() { Id = 2, Email = "test2@test.com"},
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task ValidateAsync_ForInvalidUserId_FailValidation(int userId)
        {
            //Arrange
            ChangeEmailDto changeEmailDto = new(userId, "email@email.com");

            //Act
            TestValidationResult<ChangeEmailDto> validationResult = await _validator.TestValidateAsync(changeEmailDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("test")]
        [InlineData("test@")]
        [InlineData("test@@")]
        [InlineData("test1@test.com")] //email already exists in SeedUsers()
        [InlineData("test2@test.com")] //email already exists in SeedUsers()
        public async Task ValidateAsync_ForInvalidEmail_FailValidation(string email)
        {
            //Arrange
            ChangeEmailDto changeEmailDto = new(1, email);

            //Act
            TestValidationResult<ChangeEmailDto> validationResult = await _validator.TestValidateAsync(changeEmailDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangeEmailDto changeEmailDto)
        {
            //Act
            TestValidationResult<ChangeEmailDto> validationResult = await _validator.TestValidateAsync(changeEmailDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}