using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class ChangePasswordDtoValidatorTests
    {
        private readonly ChangePasswordDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _iconfigurationMock = new();

        public ChangePasswordDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new UserService(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object);
            _validator = new ChangePasswordDtoValidator(_userService);
            SeedUsers();
        }

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

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangePasswordDto(2, "ValidPassword1", "ValidPassword1") };
            yield return new object[] { new ChangePasswordDto(2, "ValidPassword1@", "ValidPassword1@") };
            yield return new object[] { new ChangePasswordDto(1, "PasswordAccepted123@", "PasswordAccepted123@") };
            yield return new object[] { new ChangePasswordDto(3, "P123123@k", "P123123@k") };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ChangePasswordDto changePasswordDto)
        {
            //Act
            TestValidationResult<ChangePasswordDto> validationResult = await _validator.TestValidateAsync(changePasswordDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(100)]
        public async Task ValidateAsync_ForInvalidUserId_FailValidation(int userId)
        {
            //Arrange
            ChangePasswordDto changePasswordDto = new(userId, "ValidPassword1", "ValidPassword1");

            //Act
            TestValidationResult<ChangePasswordDto> validationResult = await _validator.TestValidateAsync(changePasswordDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [InlineData(" ", " ")]
        [InlineData(" 22", " 22")]
        [InlineData(" invalid password", " invalid password")]
        [InlineData("Password123@", "OtherPasswordPassword123@")]
        public async Task ValidateAsync_ForInvalidNewPassword_FailValidation(string newPassword, string confirmPassword)
        {
            //Arrange
            ChangePasswordDto changePasswordDto = new(1, newPassword, confirmPassword);

            //Act
            TestValidationResult<ChangePasswordDto> validationResult = await _validator.TestValidateAsync(changePasswordDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}