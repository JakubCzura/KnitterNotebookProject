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

public class ChangeNicknameDtoValidatorTests : IDisposable
{
    private readonly ChangeNicknameDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _iconfigurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public ChangeNicknameDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object, _sharedResourceViewModelMock.Object);
        _validator = new ChangeNicknameDtoValidator(_userService);
        SeedUsers();
    }

    public void Dispose()
    {
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public static IEnumerable<object[]> ValidData()
    {
        yield return new object[] { new ChangeNicknameDto(1, "NewNick1") };
        yield return new object[] { new ChangeNicknameDto(2, "TestNewNick") };
    }

    private void SeedUsers()
    {
        List<User> users =
        [
            new User() { Nickname = "Nick1", ThemeId = 1 },
            new User() { Nickname = "TestNick", ThemeId = 2 },
        ];
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
        ChangeNicknameDto changeNicknameDto = new(userId, "Nick1");

        //Act
        TestValidationResult<ChangeNicknameDto> validationResult = await _validator.TestValidateAsync(changeNicknameDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("InvalidTooLongNicknameeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
    public async Task ValidateAsync_ForInvalidNickname_FailValidation(string nickname)
    {
        //Arrange
        ChangeNicknameDto changeNicknameDto = new(1, nickname);

        //Act
        TestValidationResult<ChangeNicknameDto> validationResult = await _validator.TestValidateAsync(changeNicknameDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Nickname);
    }

    [Theory]
    [MemberData(nameof(ValidData))]
    public async Task ValidateAsync_ForValidData_PassValidation(ChangeNicknameDto changeNicknameDto)
    {
        //Act
        TestValidationResult<ChangeNicknameDto> validationResult = await _validator.TestValidateAsync(changeNicknameDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}