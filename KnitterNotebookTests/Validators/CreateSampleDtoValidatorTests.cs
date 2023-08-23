using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class CreateSampleDtoValidatorTests
    {
        private readonly CreateSampleDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        public CreateSampleDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object);
            _validator = new CreateSampleDtoValidator(_userService);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1 },
                new User() { Id = 2 },
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new CreateSampleDto("", 2, 3, 2, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto(null!, 2, 3, 2, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto(new string('K', 201), 2, 3, 2, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 100001, 3, 2, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, -3, 2, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 100001, 2, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 25, 0.09, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 101, 101, NeedleSizeUnit.cm, "test", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, new string('K', 10001), 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, "description is ok", -1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, "description is ok", 0, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, "description is ok", 10, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, "description is ok", 1, "") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, "description is ok", 1, "c:\\test\\test.txt") };
            yield return new object[] { new CreateSampleDto("test name", 12, 20, 10, NeedleSizeUnit.cm, "description is ok", 20, "c:\\test\\test.jpg") };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateSampleDto("Name", 2, 3, 2, NeedleSizeUnit.cm, "Description", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("Yarn Name", 1, 30, 1.5, NeedleSizeUnit.mm, "Description", 2, "c:\\test\\test.png") };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task Validate_ForInvalidData_FailValidation(CreateSampleDto createSampleDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task Validate_ForValidData_PassValidation(CreateSampleDto createSampleDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}