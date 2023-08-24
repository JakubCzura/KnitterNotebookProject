﻿using FluentValidation.TestHelper;
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
    public class RegisterUserDtoValidatorTests
    {
        private readonly RegisterUserDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();

        public RegisterUserDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object);
            _validator = new RegisterUserDtoValidator(_userService);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1, Email = "nick1@mail.com", Nickname = "Nick1"},
                new User() { Id = 2, Email = "nick2@mail.com", Nickname = "Nick2"},
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new RegisterUserDto("ValidNickname", "uservalidemail@mail.com", "Pass123@word") };
            yield return new object[] { new RegisterUserDto("Nickname", "emailuser@mail.com", "Strong321@xSd") };
        }

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
}