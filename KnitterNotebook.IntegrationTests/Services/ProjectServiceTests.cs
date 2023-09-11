using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class ProjectServiceTests
    {
        private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();

        public ProjectServiceTests()
        {
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object);
            _projectService = new(_databaseContext, _userService);
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.Migrate();
            SeedData();
        }

        private void SeedData()
        {
            User user = new()
            {
                Email = "email@email.com",
                Nickname = "Nickname",
                Password = "Password123",
                ThemeId = 1,
                Projects = new()
                {
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
                }
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
    }
}