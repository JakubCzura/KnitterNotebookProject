using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.IntegrationTests.Validators;

public class ChangePasswordDtoValidatorTests : IDisposable
{
    private readonly ChangePasswordDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _iconfigurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public ChangePasswordDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new UserService(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object, _sharedResourceViewModelMock.Object);
        _validator = new ChangePasswordDtoValidator(_userService);
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
            new User() { ThemeId = 1},
            new User() { ThemeId = 2 },
            new User() { ThemeId = 3 }
        ];
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

    [Fact]
    public async Task ValidateAsync_ForInvalidUserId_FailValidation()
    {
        //Arrange
        ChangePasswordDto changePasswordDto = new(999999, "ValidPassword1", "ValidPassword1");

        //Act
        TestValidationResult<ChangePasswordDto> validationResult = await _validator.TestValidateAsync(changePasswordDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(" ", " ")]
    [InlineData(null, " ")]
    [InlineData(" ", null)]
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