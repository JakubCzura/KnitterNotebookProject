using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.IntegrationTests.Validators
{
    public class ChangeThemeDtoValidatorTests
    {
        private readonly ChangeThemeDtoValidator _validator;
        private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
        private readonly UserService _userService;
        private readonly ThemeService _themeService;
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _iconfigurationMock = new();

        public ChangeThemeDtoValidatorTests()
        {
            _themeService = new(_databaseContext);
            _userService = new(_databaseContext, _themeService, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object);
            _validator = new ChangeThemeDtoValidator(_userService, _themeService);
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.Migrate();
            SeedThemes();
        }

        private void SeedThemes()
        {
            List<Theme> themes = new()
            {
                new Theme() { Name = ApplicationTheme.Default, Users = new List<User>() { new User() { Nickname = "Name1" } } },
                new Theme() { Name = ApplicationTheme.Light, Users = new List<User>() { new User() { Nickname = "Nickname2" } }  },
                new Theme() { Name = ApplicationTheme.Dark, Users = new List<User>() { new User() { Nickname = "Nickname3" } } }
            };
            _databaseContext.Themes.AddRange(themes);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new ChangeThemeDto(-1, ApplicationTheme.Default) };
            yield return new object[] { new ChangeThemeDto(0, ApplicationTheme.Light) };
            yield return new object[] { new ChangeThemeDto(-1, ApplicationTheme.Dark) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeThemeDto(1, ApplicationTheme.Default) };
            yield return new object[] { new ChangeThemeDto(1, ApplicationTheme.Light) };
            yield return new object[] { new ChangeThemeDto(2, ApplicationTheme.Dark) };
            yield return new object[] { new ChangeThemeDto(3, ApplicationTheme.Dark) };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task Validate_ForInvalidData_FailValidation(ChangeThemeDto changeThemeDto)
        {
            //Act
            TestValidationResult<ChangeThemeDto> validationResult = await _validator.TestValidateAsync(changeThemeDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task Validate_ForValidData_PassValidation(ChangeThemeDto changeThemeDto)
        {
            //Act
            TestValidationResult<ChangeThemeDto> validationResult = await _validator.TestValidateAsync(changeThemeDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}