using FluentAssertions;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Services;

public class ProjectServiceTests : IDisposable
{
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly ProjectService _projectService;
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public ProjectServiceTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object, _sharedResourceViewModelMock.Object);
        _projectService = new(_databaseContext, _userService);
        SeedData();
    }

    public void Dispose()
    {
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SeedData()
    {
        User user = new()
        {
            Email = "email@email.com",
            Nickname = "NicknameOfUserForTestingPurposes",
            Password = "Password123",
            ThemeId = 1,
            Projects =
            [
                new Project()
                {
                    Name = "Project1",
                    StartDate = DateTime.Today,
                    ProjectStatus = ProjectStatusName.Planned
                },
                new Project()
                {
                    Name = "Project2",
                    StartDate = DateTime.Today.AddDays(6),
                    ProjectStatus = ProjectStatusName.Planned
                },
                new Project()
                {
                    Name = "Project3",
                    StartDate = DateTime.Today.AddDays(-1),
                    ProjectStatus = ProjectStatusName.InProgress
                },
                new Project()
                {
                    Name = "Project4",
                    StartDate = DateTime.Today.AddDays(-6),
                    ProjectStatus = ProjectStatusName.InProgress
                },
                new Project()
                {
                    Name = "Project5",
                    StartDate = DateTime.Today.AddDays(-20),
                    EndDate = DateTime.Today.AddDays(-10),
                    ProjectStatus = ProjectStatusName.Finished
                },
                new Project()
                {
                    Name = "Project6",
                    StartDate = DateTime.Today.AddDays(-6),
                    EndDate = DateTime.Today,
                    ProjectStatus = ProjectStatusName.Finished
                }
            ]
        };

        _databaseContext.Users.Add(user);
        _databaseContext.SaveChanges();
    }

    [Fact]
    public async Task ProjectExistsAsync_ForExistingProject_ReturnsTrue()
    {
        //Arrange
        int projectId = 1;

        //Act
        bool result = await _projectService.ProjectExistsAsync(projectId);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ProjectExistsAsync_ForNotExistingProject_ReturnsFalse()
    {
        //Arrange
        int projectId = 99999;

        //Act
        bool result = await _projectService.ProjectExistsAsync(projectId);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PlanProjectAsync_ForNullData_ThrowsNullReferenceException()
    {
        //Arrange
        PlanProjectDto dto = null!;

        //Act
        Func<Task> action = async () => await _projectService.PlanProjectAsync(dto);

        //Assert
        await action.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task PlanProjectAsync_ForNotExistingUser_ThrowsEntityNotFoundException()
    {
        //Arrange
        PlanProjectDto dto = new("Project", DateTime.Today, [], [], null, null, 99999);

        //Act
        Func<Task> action = async () => await _projectService.PlanProjectAsync(dto);

        //Assert
        await action.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task PlanProjectAsync_ForValidData_CreatesProject()
    {
        //Arrange
        string path = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "TestPdf.pdf");
        PlanProjectDto dto = new("Project",
                                DateTime.Today,
                                [new(2.5, NeedleSizeUnit.mm)],
                                [new("SampleYarn1")],
                                "Description",
                                path,
                                1);

        //Act
        int result = await _projectService.PlanProjectAsync(dto);

        //Assert
        result.Should().Be(4);
        Directory.GetFiles(Paths.UserDirectory("NicknameOfUserForTestingPurposes")).Any(x => x.EndsWith("TestPdf.pdf")).Should().BeTrue();

        //Clean up after test
        Directory.Delete(Paths.UserDirectory("NicknameOfUserForTestingPurposes"), true);
    }

    [Fact]
    public async Task PlanProjectAsync_ForValidDataWithoutPdfPattern_CreatesProject()
    {
        //Arrange
        PlanProjectDto dto = new("Project",
                                DateTime.Today,
                                [new(2.5, NeedleSizeUnit.mm)],
                                [new("SampleYarn1")],
                                "Description",
                                null,
                                1);

        //Act
        int result = await _projectService.PlanProjectAsync(dto);

        //Assert
        result.Should().Be(3);
    }

    [Fact]
    public async Task GetPlannedProjectAsync_ForExistingProject_ReturnsPlannedProject()
    {
        //Arrange
        int projectId = 1;

        //Act
        PlannedProjectDto? result = await _projectService.GetPlannedProjectAsync(projectId);

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPlannedProjectAsync_ForNotExistingProject_ReturnsNull()
    {
        //Arrange
        int projectId = 99999;

        //Act
        PlannedProjectDto? result = await _projectService.GetPlannedProjectAsync(projectId);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserPlannedProjectsAsync_ForExistingProjectsAndUser_ReturnsProjects()
    {
        //Arrange
        int userId = 1;

        //Act
        List<PlannedProjectDto> result = await _projectService.GetUserPlannedProjectsAsync(userId);

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetUserPlannedProjectsAsync_ForNotExistingUser_ReturnsEmptyList()
    {
        //Arrange
        int userId = 99999;

        //Act
        List<PlannedProjectDto> result = await _projectService.GetUserPlannedProjectsAsync(userId);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProjectInProgressAsync_ForExistingProject_ReturnsProject()
    {
        //Arrange
        int projectId = 3;

        //Act
        ProjectInProgressDto? result = await _projectService.GetProjectInProgressAsync(projectId);

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProjectInProgressAsync_ForNotExistingProject_ReturnsNull()
    {
        //Arrange
        int projectId = 99999;

        //Act
        ProjectInProgressDto? result = await _projectService.GetProjectInProgressAsync(projectId);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserProjectsInProgressAsync_ForExistingProjectsAndUser_ReturnsProjects()
    {
        //Arrange
        int userId = 1;

        //Act
        List<ProjectInProgressDto> result = await _projectService.GetUserProjectsInProgressAsync(userId);

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetUserProjectsInProgressAsync_ForNotExistingUser_ReturnsEmptyList()
    {
        //Arrange
        int userId = 99999;

        //Act
        List<ProjectInProgressDto> result = await _projectService.GetUserProjectsInProgressAsync(userId);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFinishedProjectAsync_ForExistingProject_ReturnsProject()
    {
        //Arrange
        int projectId = 5;

        //Act
        FinishedProjectDto? result = await _projectService.GetFinishedProjectAsync(projectId);

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFinishedProjectAsync_ForNotExistingProject_ReturnsNull()
    {
        //Arrange
        int projectId = 99999;

        //Act
        FinishedProjectDto? result = await _projectService.GetFinishedProjectAsync(projectId);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserFinishedProjectsAsync_ForExistingProjectsAndUser_ReturnsProjects()
    {
        //Arrange
        int userId = 1;

        //Act
        List<FinishedProjectDto> result = await _projectService.GetUserFinishedProjectsAsync(userId);

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetUserFinishedProjectsAsync_ForNotExistingUser_ReturnsEmptyList()
    {
        //Arrange
        int userId = 99999;

        //Act
        List<FinishedProjectDto> result = await _projectService.GetUserFinishedProjectsAsync(userId);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ChangeProjectStatus_ForNullData_ThrowsInvalidOperationException()
    {
        //Arrange
        ChangeProjectStatusDto dto = null!;

        //Act
        Func<Task> action = async () => await _projectService.ChangeProjectStatus(dto);

        //Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ChangeProjectStatus_ForNotExistingProject_Returns0()
    {
        //Arrange
        ChangeProjectStatusDto dto = new(99999, ProjectStatusName.InProgress);

        //Act
        int result = await _projectService.ChangeProjectStatus(dto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ChangeProjectStatus_ForExistingProject_ChangesProjectStatus()
    {
        //Arrange
        ChangeProjectStatusDto dto = new(1, ProjectStatusName.InProgress);

        //Act
        int result = await _projectService.ChangeProjectStatus(dto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task ChangeUserPlannedProjectsToProjectsInProgressDueToDate_ForNotExistingUser_Returns0()
    {
        //Arrange
        int userId = 99999;

        //Act
        int result = await _projectService.ChangeUserPlannedProjectsToProjectsInProgressDueToDate(userId);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ChangeUserPlannedProjectsToProjectsInProgressDueToDate_ForValidData_ChangesProjectsStatuses()
    {
        //Arrange
        //There is one planned project with matching start date in database inserted in SeedData() method
        int userId = 1;

        //Act
        int result = await _projectService.ChangeUserPlannedProjectsToProjectsInProgressDueToDate(userId);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task EditProjectAsync_ForValidData_EditsProject()
    {
        //Arrange
        string path = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "TestPdf.pdf");
        EditProjectDto dto = new(1,
                                "Project's name",
                                DateTime.Today.AddDays(1),
                                [new(3.5, NeedleSizeUnit.mm), new(6.5, NeedleSizeUnit.cm)],
                                [new("Merino"), new("Soft Sheep")],
                                "Description",
                                path,
                                1);

        //Act
        int result = await _projectService.EditProjectAsync(dto);

        //Assert
        result.Should().Be(6);
        Directory.GetFiles(Paths.UserDirectory("NicknameOfUserForTestingPurposes")).Any(x => x.EndsWith("TestPdf.pdf")).Should().BeTrue();

        //Clean up after test
        Directory.Delete(Paths.UserDirectory("NicknameOfUserForTestingPurposes"), true);
    }

    [Fact]
    public async Task EditProjectAsync_ForValidDataWithoutPatternPdf_EditsProject()
    {
        //Arrange
        EditProjectDto dto = new(1,
                                "Project's name",
                                DateTime.Today.AddDays(1),
                                [new(3.5, NeedleSizeUnit.mm), new(6.5, NeedleSizeUnit.cm)],
                                [new("Merino"), new("Soft Sheep")],
                                "Description",
                                null,
                                1);

        //Act
        int result = await _projectService.EditProjectAsync(dto);

        //Assert
        result.Should().Be(5);
    }

    [Fact]
    public async Task EditProjectAsync_ForNotExistingProject_Returns0()
    {
        //Arrange
        EditProjectDto dto = new(99999,
                                "Project's name",
                                DateTime.Today.AddDays(1),
                                [new(3.5, NeedleSizeUnit.mm), new(6.5, NeedleSizeUnit.cm)],
                                [new("Merino"), new("Soft Sheep")],
                                "Description",
                                null,
                                1);

        //Act
        int result = await _projectService.EditProjectAsync(dto);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task EditProjectAsync_ForNotExistingUser_ThrowsEntityNotFoundException()
    {
        //Arrange
        EditProjectDto dto = new(1,
                                "Project's name",
                                DateTime.Today.AddDays(1),
                                [new(3.5, NeedleSizeUnit.mm), new(6.5, NeedleSizeUnit.cm)],
                                [new("Merino"), new("Soft Sheep")],
                                "Description",
                                null,
                                99999);

        //Act
        Func<Task> action = async () => await _projectService.EditProjectAsync(dto);

        //Assert
        await action.Should().ThrowAsync<EntityNotFoundException>();
    }
}