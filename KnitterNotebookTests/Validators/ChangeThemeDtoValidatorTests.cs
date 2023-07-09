using FluentValidation;
using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebookTests.Validators
{
    public class ChangeThemeDtoValidatorTests
    {
        private readonly IValidator<ChangeThemeDto> _validator;
        private readonly DatabaseContext _databaseContext;

        public ChangeThemeDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new ChangeThemeDtoValidator(_databaseContext);
            SeedThemes();
        }

        private void SeedThemes()
        {
            List<Theme> themes = new()
            {
                new Theme() { Name = "Default", Users = new List<User>() { new User() { Id = 1 } } },
                new Theme() { Name = "Light", Users = new List<User>() { new User() { Id = 2 } }  },
                new Theme() { Name = "Dark", Users = new List<User>() { new User() { Id = 3 } } }
            };
            _databaseContext.Themes.AddRange(themes);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new ChangeThemeDto(-1, "Default") };
            yield return new object[] { new ChangeThemeDto(0, "Light") };
            yield return new object[] { new ChangeThemeDto(-1, "Dark") };
            yield return new object[] { new ChangeThemeDto(1, "DefaultLightTheme") };
            yield return new object[] { new ChangeThemeDto(-1, "NonExistingTheme") };
            yield return new object[] { new ChangeThemeDto(0, "Test") };
            yield return new object[] { new ChangeThemeDto(2, string.Empty) };
            yield return new object[] { new ChangeThemeDto(2, null!) };
            yield return new object[] { new ChangeThemeDto(-1, null!) };
            yield return new object[] { new ChangeThemeDto(-1, string.Empty) };
            yield return new object[] { new ChangeThemeDto(34, "Default") };
            yield return new object[] { new ChangeThemeDto(4, "Light") };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new ChangeThemeDto(1, "Default") };
            yield return new object[] { new ChangeThemeDto(1, "Light") };
            yield return new object[] { new ChangeThemeDto(2, "Dark") };    
            yield return new object[] { new ChangeThemeDto(3, "Default") };    
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task Validate_ForInvalidData_FailValidation(ChangeThemeDto changeThemeDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changeThemeDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task Validate_ForValidData_PassValidation(ChangeThemeDto changeThemeDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(changeThemeDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}