using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.IntegrationTests.Validators;

public class ResetPasswordDtoValidatorTests : IDisposable
{
    private readonly ResetPasswordDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _ThemeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _iconfigurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public ResetPasswordDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _ThemeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object, _sharedResourceViewModelMock.Object);
        _validator = new ResetPasswordDtoValidator(_userService);
        SeedUsers();
    }

    public void Dispose()
    {
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SeedUsers()
    {
        List<User> users =
        [
            new User()
            {
                Email = "nick1@mail.com",
                Nickname = "Nick1",
                PasswordResetToken = "123xpklo2",
                PasswordResetTokenExpirationDate = DateTime.UtcNow.AddDays(1),
                ThemeId = 1
            }
        ];
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    [Fact]
    public async Task ValidateAsync_ForValidData_PassValidation()
    {
        //Arrange
        ResetPasswordDto resetPasswordDto = new("nick1@mail.com", "123xpklo2", "PasswordNew123", "PasswordNew123");

        //Act
        TestValidationResult<ResetPasswordDto> validationResult = await _validator.TestValidateAsync(resetPasswordDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
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
    [InlineData("123")]
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
    [InlineData("validPassword123", " ")]
    [InlineData("validPassword123", "other password")]
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