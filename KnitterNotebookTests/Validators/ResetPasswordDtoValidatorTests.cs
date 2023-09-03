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
    public class ResetPasswordDtoValidatorTests
    {
        private readonly ResetPasswordDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _ThemeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _iconfigurationMock = new();

        public ResetPasswordDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _ThemeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object);
            _validator = new ResetPasswordDtoValidator(_userService);
            SeedUsers();
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ResetPasswordDto("nick1@mail.com", "123xpklo2", "PasswordNew123", "PasswordNew123") };
            yield return new object[] { new ResetPasswordDto("nick2@mail.com", "4213x3123", "PasswordNew123", "PasswordNew123") };
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User()
                {
                    Id = 1,
                    Email = "nick1@mail.com",
                    Nickname = "Nick1",
                    PasswordResetToken = "123xpklo2",
                    PasswordResetTokenExpirationDate = DateTime.UtcNow.AddDays(1)
                },
                new User()
                {
                    Id = 2,
                    Email = "nick2@mail.com",
                    Nickname = "Nick2",
                    PasswordResetToken = "4213x3123",
                    PasswordResetTokenExpirationDate = DateTime.UtcNow.AddHours(2)
                }
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(ResetPasswordDto resetPasswordDto)
        {
            //Act
            TestValidationResult<ResetPasswordDto> validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" 123 ")]
        [InlineData(" 123 @")]
        [InlineData("email@email.com")]
        public async Task ValidateAsync_ForInvalidEmail_FailValidation(string email)
        {
            //Arrange
            ResetPasswordDto resetPasswordDto = new(email, "123xpklo2", "PasswordNew123", "PasswordNew123");

            //Act
            TestValidationResult<ResetPasswordDto> validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" invalid")]
        [InlineData("231233213213123213121213")]
        public async Task ValidateAsync_ForInvalidPasswordResetToken_FailValidation(string token)
        {
            //Arrange
            ResetPasswordDto resetPasswordDto = new("email@email.com", token, "PasswordNew123", "PasswordNew123");

            //Act
            TestValidationResult<ResetPasswordDto> validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Token);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("123456789")]
        [InlineData("KJ")]
        public async Task ValidateAsync_ForInvalidNewPassword_FailValidation(string password)
        {
            //Arrange
            ResetPasswordDto resetPasswordDto = new("email@email.com", "123xpklo2", password, "PasswordNew123");

            //Act
            TestValidationResult<ResetPasswordDto> validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Theory]
        [InlineData("validPassword123", null)]
        [InlineData("validPassword123", "")]
        [InlineData("validPassword123", " ")]
        [InlineData("validPassword123", "other password")]
        [InlineData("validPassword123", "againOtherPassword123")]
        public async Task ValidateAsync_ForNewPasswordNotEqualRepeatedNewPassword_FailValidation(string newPassword, string newRepeatedPassword)
        {
            //Arrange
            ResetPasswordDto resetPasswordDto = new("email@email.com", "123xpklo2", newPassword, newRepeatedPassword);

            //Act
            TestValidationResult<ResetPasswordDto> validationResult = await _validator.TestValidateAsync(resetPasswordDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}