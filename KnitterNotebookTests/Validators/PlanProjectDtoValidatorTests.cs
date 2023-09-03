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
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.Validators
{
    public class PlanProjectDtoValidatorTests
    {
        private readonly PlanProjectDtoValidator _validator;
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _iconfigurationMock = new();

        public PlanProjectDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object);
            _validator = new PlanProjectDtoValidator(_userService);
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

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new PlanProjectDto(
                                        "Knitting project",
                                        null,
                                        new List<CreateNeedleDto>(){ new CreateNeedleDto(2.5, NeedleSizeUnit.mm) },
                                        new List<CreateYarnDto>(){ new CreateYarnDto("My favourite yarn") },
                                        "Sample description",
                                        null,
                                        1) };
            yield return new object[] { new PlanProjectDto(
                                        "My project",
                                        DateTime.Today,
                                        new List<CreateNeedleDto>(){ new CreateNeedleDto(1, NeedleSizeUnit.cm), new CreateNeedleDto (2, NeedleSizeUnit.mm)},
                                        new List<CreateYarnDto>(){ new CreateYarnDto("Cotton yarn") },
                                        null,
                                        null,
                                        1) };
            yield return new object[] { new PlanProjectDto(
                                        "Sample project",
                                        DateTime.Today.AddDays(2),
                                        new List<CreateNeedleDto>(){ new CreateNeedleDto(4, NeedleSizeUnit.cm) },
                                        new List<CreateYarnDto>(){ new CreateYarnDto("Woolen yarn") },
                                        "Description of my project",
                                        @"c:\users\user\files\projectPattern.pdf",
                                        2) };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(PlanProjectDto planProjectDto)
        {
            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null!)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars More Than 100 chars")]
        public async Task ValidateAsync_ForInvalidName_FailValidation(string name)
        {
            //Arrange
            PlanProjectDto planProjectDto = new(name, DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 1);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidStartDate_FailValidation()
        {
            //Arrange
            PlanProjectDto planProjectDto = new("Name", DateTime.Today.AddDays(-1), Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 1);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        public static IEnumerable<object[]> InvalidNeedlesData()
        {
            yield return new object[] { null! };
            yield return new object[] { new List<CreateNeedleDto>() };
        }

        [Theory]
        [MemberData(nameof(InvalidNeedlesData))]
        public async Task ValidateAsync_ForInvalidNeedles_FailValidation(List<CreateNeedleDto> createNeedleDtos)
        {
            //Arrange
            PlanProjectDto planProjectDto = new("Name", DateTime.Today, createNeedleDtos, Enumerable.Empty<CreateYarnDto>(), null, null, 1);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Needles);
        }

        public static IEnumerable<object[]> InvalidYarnsData()
        {
            yield return new object[] { null! };
            yield return new object[] { new List<CreateYarnDto>() };
        }

        [Theory]
        [MemberData(nameof(InvalidYarnsData))]
        public async Task ValidateAsync_ForInvalidYarns_FailValidation(List<CreateYarnDto> yarns)
        {
            //Arrange
            PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), yarns, null, null, 1);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Yarns);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidDescription_FailValidation()
        {
            //Arrange
            PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), new string('K', 301), null, 2);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidSourcePatternPdfPath_FailValidation()
        {
            //Arrange
            PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, @"c:\users\user\files\file.jpeg", 2);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.SourcePatternPdfPath);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidUserId_FailValidation()
        {
            //Arrange
            //User with id 6 does not exist in SeedUsers()
            PlanProjectDto planProjectDto = new("Name", DateTime.Today, Enumerable.Empty<CreateNeedleDto>(), Enumerable.Empty<CreateYarnDto>(), null, null, 6);

            //Act
            TestValidationResult<PlanProjectDto> validationResult = await _validator.TestValidateAsync(planProjectDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}