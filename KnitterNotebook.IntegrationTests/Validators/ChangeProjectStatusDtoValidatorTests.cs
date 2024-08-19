using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using Moq;

namespace KnitterNotebook.IntegrationTests.Validators;

public class ChangeProjectStatusDtoValidatorTests : IDisposable
{
    private readonly ChangeProjectStatusDtoValidator _validator;
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly ProjectService _projectService;
    private readonly Mock<IUserService> _userServiceMock = new();

    public ChangeProjectStatusDtoValidatorTests()
    {
        _databaseContext.Database.EnsureCreated();
        _projectService = new(_databaseContext, _userServiceMock.Object);
        _validator = new ChangeProjectStatusDtoValidator(_projectService);
        SeedProjects();
    }

    public void Dispose()
    {
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SeedProjects()
    {
        List<User> users =
        [
            new User()
            {
                ThemeId = 1,
                Projects =
                [
                    new Project() { Name = "Project1", },
                    new Project() { Name = "Project2" },
                ]
            },
            new User()
            {
                ThemeId = 2,
                Projects =
                [
                    new Project() { Name = "Project3", },
                    new Project() { Name = "Project4" },
                ]
            },
        ];
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    public static IEnumerable<object[]> ValidData()
    {
        yield return new object[] { new ChangeProjectStatusDto(1, ProjectStatusName.Planned) };
        yield return new object[] { new ChangeProjectStatusDto(1, ProjectStatusName.InProgress) };
        yield return new object[] { new ChangeProjectStatusDto(2, ProjectStatusName.Finished) };
    }

    [Theory]
    [MemberData(nameof(ValidData))]
    public async Task ValidateAsync_ForValidData_PassValidation(ChangeProjectStatusDto changeProjectStatusDto)
    {
        // Act
        TestValidationResult<ChangeProjectStatusDto> validationResult = await _validator.TestValidateAsync(changeProjectStatusDto);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ValidateAsync_ForInvalidProjectId_FailValidation()
    {
        //Arrange
        ChangeProjectStatusDto changeProjectStatusDto = new(9999, ProjectStatusName.Planned);

        // Act
        TestValidationResult<ChangeProjectStatusDto> validationResult = await _validator.TestValidateAsync(changeProjectStatusDto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public async Task ValidateAsync_ForInvalidProjectStatusName_FailValidation()
    {
        //Arrange
        ChangeProjectStatusDto changeProjectStatusDto = new(1, (ProjectStatusName)100);

        // Act
        TestValidationResult<ChangeProjectStatusDto> validationResult = await _validator.TestValidateAsync(changeProjectStatusDto);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.ProjectStatus);
    }
}