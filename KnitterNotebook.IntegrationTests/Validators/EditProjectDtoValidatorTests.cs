using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Validators;

public class EditProjectDtoValidatorTests
{
    private readonly EditProjectDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly UserService _userService;
    private readonly ProjectService _projectService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public EditProjectDtoValidatorTests()
    {
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object, _sharedResourceViewModelMock.Object);
        _projectService = new(_databaseContext, _userService);
        _validator = new EditProjectDtoValidator(_userService, _projectService);
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Database.Migrate();
        SeedProjects();
    }

    private void SeedProjects()
    {
        List<User> users = new()
        {
            new User()
            {
                ThemeId = 1,
                Projects = new()
                {
                    new Project()
                    {
                        Name = "Project1",
                        StartDate = DateTime.UtcNow.AddDays(1),
                        Needles = new()
                        {
                            new(2.5, NeedleSizeUnit.mm),
                            new(3.5, NeedleSizeUnit.cm),
                            new(6, NeedleSizeUnit.mm),
                        },
                        Yarns = new()
                        {
                            new("Merino"),
                            new("Yarn"),
                            new("Super Yarn"),
                        },
                        Description = "Sample description of planned project",
                        ProjectStatus = ProjectStatusName.Planned,
                        PatternPdf = null
                    }
                },
            }
        };
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    [Fact]
    public async Task ValidateAsync_ForValidData_PassValidation()
    {
        //Arrange
        EditProjectDto editPlannedProjectDto = new(1,
            "Project new name",
            DateTime.UtcNow.AddDays(1),
            new List<CreateNeedleDto>()
            {
                new(2.5, NeedleSizeUnit.mm),
                new(0.5, NeedleSizeUnit.cm),
                new(6.5, NeedleSizeUnit.mm),
            },
            new List<CreateYarnDto>()
            {
                new("Merino"),
                new("Super yarn"),
            },
            "New description",
            @"c:\computer\user\file.pdf",
            1);

        //Act
        TestValidationResult<EditProjectDto> validationResult = await _validator.TestValidateAsync(editPlannedProjectDto);

        //Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ValidateAsync_ForNotExistingProject_FailValidation()
    {
        //Arrange
        EditProjectDto editPlannedProjectDto = new(99999, "Project new name", DateTime.UtcNow.AddDays(1), new List<CreateNeedleDto>(), new List<CreateYarnDto>(), null, null, 1);

        //Act
        TestValidationResult<EditProjectDto> validationResult = await _validator.TestValidateAsync(editPlannedProjectDto);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public async Task ValidateAsync_ForFailingPlanProjectDtoValidator_FailValidation()
    {
        //Arrange
        EditProjectDto editPlannedProjectDto = new(1, new string('K', 1000), DateTime.UtcNow.AddDays(-10), new List<CreateNeedleDto>(), new List<CreateYarnDto>(), null, null, 99999);

        //Act
        TestValidationResult<EditProjectDto> validationResult = await _validator.TestValidateAsync(editPlannedProjectDto);

        //Assert
        //Not failing because of Id as it is valid, but other properties validated by PlanProjectDtoValidator are invalid
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Id);
        validationResult.ShouldHaveAnyValidationError();
    }
}