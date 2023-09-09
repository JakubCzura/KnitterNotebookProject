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
    public class CreateSampleDtoValidatorTests
    {
        private readonly CreateSampleDtoValidator _validator;
        private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();

        public CreateSampleDtoValidatorTests()
        {
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object);
            _validator = new CreateSampleDtoValidator(_userService);
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.Migrate();
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Nickname = "Nickname1", ThemeId = 1 },
                new User() { Nickname = "Nickname2", ThemeId = 2 },
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateSampleDto("Name", 2, 3, 2, NeedleSizeUnit.cm, "Description", 1, "c:\\test\\test.jpg") };
            yield return new object[] { new CreateSampleDto("Yarn Name", 1, 30, 1.5, NeedleSizeUnit.mm, "Description", 2, "c:\\test\\test.png") };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(CreateSampleDto createSampleDto)
        {
            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        public static IEnumerable<object[]> InvalidYarnNames()
        {
            yield return new object[] { "" };
            yield return new object[] { null! };
            yield return new object[] { new string('K', 201) };
        }

        [Theory]
        [MemberData(nameof(InvalidYarnNames))]
        public async Task ValidateAsync_ForInvalidYarnName_FailValidation(string yarnName)
        {
            //Arrange
            CreateSampleDto createSampleDto = new(yarnName, 2, 3, 2, NeedleSizeUnit.cm, "Description", 1, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.YarnName);
        }

        [Theory]
        [InlineData(-1), InlineData(0), InlineData(100001)]
        public async Task ValidateAsync_ForInvalidLoopsQuantity_FailValidation(int loopsQuantity)
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", loopsQuantity, 3, 2, NeedleSizeUnit.cm, "Description", 1, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.LoopsQuantity);
        }

        [Theory]
        [InlineData(-1), InlineData(0), InlineData(100001)]
        public async Task ValidateAsync_ForInvalidRowsQuantity_FailValidation(int rowsQuantity)
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", 2, rowsQuantity, 2, NeedleSizeUnit.cm, "Description", 1, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.RowsQuantity);
        }

        [Theory]
        [InlineData(0.09), InlineData(101)]
        public async Task ValidateAsync_ForInvalidNeedleSize_FailValidation(double needleSize)
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", 2, 3, needleSize, NeedleSizeUnit.cm, "Description", 1, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.NeedleSize);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidNeedleSizeUnit_FailValidation()
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", 2, 3, 2, (NeedleSizeUnit)99999, "Description", 1, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.NeedleSizeUnit);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidDescription_FailValidation()
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", 2, 3, 2, NeedleSizeUnit.cm, new string('K', 1001), 1, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidSourceImagePath_FailValidation()
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", 2, 3, 2, NeedleSizeUnit.cm, "Description", 1, @"c:\test\test.mp3");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.SourceImagePath);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidUserId_FailValidation()
        {
            //Arrange
            CreateSampleDto createSampleDto = new("Name", 2, 3, 2, NeedleSizeUnit.cm, "Description", 999999, @"c:\test\test.jpg");

            //Act
            TestValidationResult<CreateSampleDto> validationResult = await _validator.TestValidateAsync(createSampleDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}