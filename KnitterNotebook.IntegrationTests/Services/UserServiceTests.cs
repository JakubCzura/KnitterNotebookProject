using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KnitterNotebook.IntegrationTests.Services;

public class UserServiceTests
{
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly ThemeService _themeService;
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;
    private readonly UserService _userService;
    private readonly SharedResourceViewModel _sharedResourceViewModel;

    public UserServiceTests()
    {
        Dictionary<string, string> myConfiguration = new() { { "Tokens:ResetPasswordTokenExpirationDays", "1" } };
        IConfigurationRoot configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration!).Build();

        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Database.Migrate();

        _themeService = new(_databaseContext);
        _passwordService = new();
        _tokenService = new();
        _sharedResourceViewModel = new();
        _userService = new(_databaseContext, _themeService, _passwordService, _tokenService, configuration, _sharedResourceViewModel);
        SeedUsers();
    }

    private void SeedUsers()
    {
        List<Theme> themes = new()
        {
            new() { Name = ApplicationTheme.Default },
            new() { Name = ApplicationTheme.Light },
            new() { Name = ApplicationTheme.Dark },
        };
        _databaseContext.Themes.AddRange(themes);

        List<User> users = new()
        {
            new()
            {
                Email = "user1@mail.com",
                Nickname = "Nickname1",
                Password = _passwordService.HashPassword("Password123"),
                PasswordResetToken = "PasswordResetToken1",
                PasswordResetTokenExpirationDate = DateTime.UtcNow.AddDays(1),
                ThemeId = 1
            },
            new()
            {
                Email = "user2@mail.com",
                Nickname = "Nickname2",
                Password = _passwordService.HashPassword("Password123123"),
                PasswordResetToken = "PasswordResetToken2",
                PasswordResetTokenExpirationDate = DateTime.UtcNow.AddDays(1),
                ThemeId = 2
            },
        };
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    [Fact]
    public async Task IsNicknameTakenAsync_ForExistingNickname_ReturnsTrue()
    {
        //Arrange
        string nickname = "Nickname1";

        //Act
        bool result = await _userService.IsNicknameTakenAsync(nickname);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsNicknameTakenAsync_ForNotExistingNickname_ReturnsFalse()
    {
        //Arrange
        string nickname = "Nickname3323112313213";

        //Act
        bool result = await _userService.IsNicknameTakenAsync(nickname);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsEmailTakenAsync_ForExistingEmail_ReturnsTrue()
    {
        //Arrange
        string email = "user1@mail.com";

        //Act
        bool result = await _userService.IsEmailTakenAsync(email);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailTakenAsync_ForNotExistingEmail_ReturnsFalse()
    {
        //Arrange
        string email = "tes222t@dsadsadas321321312321mail.com";

        //Act
        bool result = await _userService.IsEmailTakenAsync(email);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ArePasswordResetTokenAndExpirationDateValidAsync_ForValidTokenAndTokenExpirationDate_ReturnsTrue()
    {
        //Arrange
        string token = "PasswordResetToken1";

        //Act
        bool result = await _userService.ArePasswordResetTokenAndExpirationDateValidAsync(token);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ArePasswordResetTokenAndExpirationDateValidAsync_ForInvalidToken_ReturnsFalse()
    {
        //Arrange
        string token = "TokenNotInDatabase132132131212121231213321";

        //Act
        bool result = await _userService.ArePasswordResetTokenAndExpirationDateValidAsync(token);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ArePasswordResetTokenAndExpirationDateValidAsync_ForInvalidTokenExpirationDate_ReturnsFalse()
    {
        //Arrange
        //I want to simulate situation when token is valid but expiration date is not.
        //Token's expiration date is in the past so the situation presents user who has not reset password in time.

        string token = "123098123098123098";
        DateTime passwordResetTokenExpirationDate = DateTime.UtcNow.AddDays(-1);

        User user = new()
        {
            Email = "emailtest@test.com",
            Nickname = "Nickname12311123",
            Password = "Password1233213",
            PasswordResetToken = token,
            PasswordResetTokenExpirationDate = passwordResetTokenExpirationDate,
            ThemeId = 1
        };

        _databaseContext.Users.Add(user);
        await _databaseContext.SaveChangesAsync();

        //Act
        bool result = await _userService.ArePasswordResetTokenAndExpirationDateValidAsync(token);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UserExistsAsync_ForExistingUser_ReturnsTrue()
    {
        //Arrange
        int id = 1;

        //Act
        bool result = await _userService.UserExistsAsync(id);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UserExistsAsync_ForNotExistingUser_ReturnsFalse()
    {
        //Arrange
        int id = 999999;

        //Act
        bool result = await _userService.UserExistsAsync(id);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAsync_ForExistingUser_ReturnsUserDto()
    {
        //Based on SeedUsers() method
        //Arrange
        int id = 1;

        //Act
        UserDto? result = await _userService.GetAsync(id);

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAsync_ForNotExistingUser_ReturnsNull()
    {
        //Arrange
        int id = 999999;

        //Act
        UserDto? result = await _userService.GetAsync(id);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LogInAsync_ForValidCredentials_ReturnsUserId()
    {
        //Arrange
        LogInDto logInDto = new("user1@mail.com", "Password123");

        //Act
        int? result = await _userService.LogInAsync(logInDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task LogInAsync_ForInvalidEmail_ReturnsNull()
    {
        //Arrange
        LogInDto logInDto = new("emailNotInDatabase@mail.com", "Password123123");

        //Act
        int? result = await _userService.LogInAsync(logInDto);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LogInAsync_ForValidEmailButInvalidPassword_ReturnsNull()
    {
        //Arrange
        LogInDto logInDto = new("user1@mail.com", "InvalidPassword123");

        //Act
        int? result = await _userService.LogInAsync(logInDto);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ForValidData_AddsNewUserToDatabase()
    {
        //Arrange
        RegisterUserDto registerUserDto = new("Nickname123KKK", "email123123@email123.com", "Password123KJ");

        //Act
        int result = await _userService.CreateAsync(registerUserDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ForNullData_ThrowsNullReferenceException()
    {
        //Arrange
        RegisterUserDto registerUserDto = null!;

        //Act
        Func<Task> action = async () => await _userService.CreateAsync(registerUserDto);

        //Assert
        await action.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task GetNickNameAsync_ForExistingUser_ReturnsNickname()
    {
        //Arrange
        int id = 1;

        //Act
        string? result = await _userService.GetNicknameAsync(id);

        //Assert
        result.Should().Be("Nickname1");
    }

    [Fact]
    public async Task GetNickNameAsync_ForNotExistingUser_ReturnsNull()
    {
        //Arrange
        int id = 999999;

        //Act
        string? result = await _userService.GetNicknameAsync(id);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ChangePasswordAsync_ForNullData_ThrowsInvalidOperationException()
    {
        //Arrange
        ChangePasswordDto changePasswordDto = null!;

        //Act
        Func<Task> action = async () => await _userService.ChangePasswordAsync(changePasswordDto);

        //Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ChangePasswordAsync_ForNotExistingUser_Returns0()
    {
        //Arrange
        ChangePasswordDto changePasswordDto = new(999999, "NewPassword123", "NewPassword123");

        //Act
        int result = await _userService.ChangePasswordAsync(changePasswordDto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ChangeNicknameAsync_ForValidData_ChangesNickname()
    {
        //Arrange
        ChangeNicknameDto changeNicknameDto = new(1, "NewNickname123");

        //Act
        int result = await _userService.ChangeNicknameAsync(changeNicknameDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task ChangeNicknameAsync_ForNullData_ThrowsInvalidOperationException()
    {
        //Arrange
        ChangeNicknameDto changeNicknameDto = null!;

        //Act
        Func<Task> action = async () => await _userService.ChangeNicknameAsync(changeNicknameDto);

        //Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ChangeNicknameAsync_ForNotExistingUser_Returns0()
    {
        //Arrange
        ChangeNicknameDto changeNicknameDto = new(999999, "NewNickname123");

        //Act
        int result = await _userService.ChangeNicknameAsync(changeNicknameDto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ChangeEmailAsync_ForValidData_ChangesEmail()
    {
        //Arrange
        ChangeEmailDto changeEmailDto = new(1, "valid@email.com");

        //Act
        int result = await _userService.ChangeEmailAsync(changeEmailDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task ChangeEmailAsync_ForNullData_ThrowsInvalidOperationException()
    {
        //Arrange
        ChangeEmailDto changeEmailDto = null!;

        //Act
        Func<Task> action = async () => await _userService.ChangeEmailAsync(changeEmailDto);

        //Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ChangeEmailAsync_ForNotExistingUser_Returns0()
    {
        //Arrange
        ChangeEmailDto changeEmailDto = new(999999, "email@email.com");

        //Act
        int result = await _userService.ChangeEmailAsync(changeEmailDto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ChangeThemeAsync_ForValidData_ChangesTheme()
    {
        //Arrange
        ChangeThemeDto changeThemeDto = new(1, ApplicationTheme.Light);

        //Act
        int result = await _userService.ChangeThemeAsync(changeThemeDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task ChangeThemeAsync_ForNullData_ThrowsNullReferenceException()
    {
        //Arrange
        ChangeThemeDto changeThemeDto = null!;

        //Act
        Func<Task> action = async () => await _userService.ChangeThemeAsync(changeThemeDto);

        //Assert
        await action.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task ChangeThemeAsync_ForNotExistingUser_Returns0()
    {
        //Arrange
        ChangeThemeDto changeThemeDto = new(999999, ApplicationTheme.Light);

        //Act
        int result = await _userService.ChangeThemeAsync(changeThemeDto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ChangeThemeAsync_NotExistingTheme_ThrowsEntityNotFoundException()
    {
        //Arrange
        ChangeThemeDto changeThemeDto = new(1, (ApplicationTheme)999);

        //Act
        Func<Task> action = async () => await _userService.ChangeThemeAsync(changeThemeDto);

        //Assert
        await action.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ResetPasswordAsync_ForValidData_ChangesPassword()
    {
        //Arrange
        ResetPasswordDto resetPasswordDto = new("user1@mail.com", "PasswordResetToken1", "NewPassword123", "NewPassword123");

        //Act
        int result = await _userService.ResetPasswordAsync(resetPasswordDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task ResetPasswordAsync_ForNotExistingUser_Returns0()
    {
        //Arrange
        ResetPasswordDto resetPasswordDto = new("emailnotindatabase@emailnotindatabase.com", "Token", "NewPassword", "NewPassword");

        //Act
        int result = await _userService.ResetPasswordAsync(resetPasswordDto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ResetPasswordAsync_ForNullData_ThrowsInvalidOperationException()
    {
        //Arrange
        ResetPasswordDto resetPasswordDto = null!;

        //Act
        Func<Task> action = async () => await _userService.ResetPasswordAsync(resetPasswordDto);

        //Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdatePasswordResetTokenStatusAsync_ForValidData_UpdatesToken()
    {
        //Arrange
        string email = "user1@mail.com";

        //Act
        (string, DateTime) result = await _userService.UpdatePasswordResetTokenStatusAsync(email);

        //Assert
        result.Item1.Should().NotBeNullOrEmpty();
        result.Item2.Should().BeAfter(DateTime.UtcNow);
    }

    [Theory]
    [InlineData(null), InlineData("emailnotindatabase@emailnotindatabase.com")]
    public async Task UpdatePasswordResetTokenStatusAsync_ForNotExistingUser_ThrowsEntityNotFoundException(string email)
    {
        //Act
        Func<Task> action = async () => await _userService.UpdatePasswordResetTokenStatusAsync(email);

        //Assert
        await action.Should().ThrowAsync<EntityNotFoundException>();
    }
}