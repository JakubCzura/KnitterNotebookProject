﻿using FluentValidation.TestHelper;
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

namespace KnitterNotebook.IntegrationTests.Validators;

public class RegisterUserDtoValidatorTests : IDisposable
{
    private readonly RegisterUserDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _iconfigurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public RegisterUserDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object, _sharedResourceViewModelMock.Object);
        _validator = new RegisterUserDtoValidator(_userService);
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
            new User() { Email = "nick1@mail.com", Nickname = "Nick1", ThemeId = 1 },
            new User() { Email = "nick2@mail.com", Nickname = "Nick2", ThemeId = 1 },
        ];
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    public static TheoryData<RegisterUserDto> ValidData => new()
    {
        { new RegisterUserDto("ValidNickname", "uservalidemail@mail.com", "Pass123@word") },
        { new RegisterUserDto("Nickname", "emailuser@mail.com", "Strong321@xSd") }
    };

    [Theory]
    [MemberData(nameof(ValidData))]
    public async Task ValidateAsync_ForValidData_PassValidation(RegisterUserDto registerUserDto)
    {
        //Act
        TestValidationResult<RegisterUserDto> validationResult = await _validator.TestValidateAsync(registerUserDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("^")]
    [InlineData("Nick1")] //Nickname already exists in SeedUsers()
    [InlineData("Nick2")] //Nickname already exists in SeedUsers()
    public async Task ValidateAsync_ForInvalidNickname_FailValidation(string nickname)
    {
        //Arrange
        RegisterUserDto registerUserDto = new(nickname, "validemail@email.com", "ValidPassword123^");

        //Act
        TestValidationResult<RegisterUserDto> validationResult = await _validator.TestValidateAsync(registerUserDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Nickname);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("test")]
    [InlineData("nick1@mail.com")] //Email already exists in SeedUsers()
    [InlineData("nick2@mail.com")] //Email already exists in SeedUsers()
    public async Task ValidateAsync_ForInvalidEmail_FailValidation(string email)
    {
        //Arrange
        RegisterUserDto registerUserDto = new("Valid nickname", email, "ValidPassword123^");

        //Act
        TestValidationResult<RegisterUserDto> validationResult = await _validator.TestValidateAsync(registerUserDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("password6")]
    public async Task ValidateAsync_ForInvalidPassword_FailValidation(string password)
    {
        //Arrange
        RegisterUserDto registerUserDto = new("Valid nickname", "validemail@email.com", password);

        //Act
        TestValidationResult<RegisterUserDto> validationResult = await _validator.TestValidateAsync(registerUserDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Password);
    }
}