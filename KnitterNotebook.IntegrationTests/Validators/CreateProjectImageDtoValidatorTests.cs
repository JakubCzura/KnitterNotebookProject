using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebookTests.IntegrationTests.Validators
{
    public class CreateProjectImageDtoValidatorTests
    {
        private readonly CreateProjectImageDtoValidator _validator;
        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _iconfigurationMock = new();

        public CreateProjectImageDtoValidatorTests()
        {
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _iconfigurationMock.Object);
            _projectService = new(_databaseContext, _userService);
            _validator = new CreateProjectImageDtoValidator(_projectService, _userService);
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.Migrate();
            SeedProjects();
        }

        private void SeedProjects()
        {
            List<Project> projects = new()
            {
                new Project() { Name = "Project1", User = new() { Nickname = "Nickname1", ThemeId = 1 } },
                new Project() { Name = "Project2", User = new() { Nickname = "Nickname2", ThemeId = 1 } },
            };
            _databaseContext.Projects.AddRange(projects);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateProjectImageDto(1, @"c:\computer\files\image.jpg", 1) };
            yield return new object[] { new CreateProjectImageDto(2, @"c:\mycomputer\mydirectory\file1.png", 2) };
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(CreateProjectImageDto createProjectImageDto)
        {
            //Act
            TestValidationResult<CreateProjectImageDto> validationResult = await _validator.TestValidateAsync(createProjectImageDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidProjectId_FailValidation()
        {
            //Arrange
            //Project doesn't exist in SeedProjects()
            CreateProjectImageDto createProjectImageDto = new(3, @"c:\computer\files\image.jpg", 1);

            //Act
            TestValidationResult<CreateProjectImageDto> validationResult = await _validator.TestValidateAsync(createProjectImageDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(@"c:\computer\files\image.pdf")]
        public async Task ValidateAsync_ForInvalidImagePath_FailValidation(string imagePath)
        {
            //Arrange
            CreateProjectImageDto createProjectImageDto = new(1, imagePath, 1);

            //Act
            TestValidationResult<CreateProjectImageDto> validationResult = await _validator.TestValidateAsync(createProjectImageDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.ImagePath);
        }

        [Fact]
        public async Task ValidateAsync_ForInvalidUserId_FailValidation()
        {
            //Arrange
            //User doesn't exist in SeedProjects()
            CreateProjectImageDto createProjectImageDto = new(1, @"c:\computer\files\image.jpg", 3);

            //Act
            TestValidationResult<CreateProjectImageDto> validationResult = await _validator.TestValidateAsync(createProjectImageDto);

            //Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}