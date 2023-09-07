using FluentAssertions;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class ProjectImageServiceTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ProjectImageService _projectImageService;
        private readonly UserService _userService;
        private readonly Mock<IThemeService> _themeServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();
        public ProjectImageServiceTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object);
            _projectImageService = new(_databaseContext, _userService);
            SeedData();
        }

        private void SeedData()
        {
            User user = new()
            {
                Id = 1,
                Email = "test@test.com",
                Nickname = "Nickname6",
                Password = "Password123",
                Projects = new()
                {
                    new() { Id = 1, Name = "Project1", Description = "Description1", ProjectImages = new()
                    {
                        new(){ Id = 1, Path = @"c:\computer\test1.jpg", ProjectId = 1},
                        new(){ Id = 2, Path = @"c:\computer\test2.jpg", ProjectId = 1},
                        new(){ Id = 3, Path = @"c:\computer\test3.jpg", ProjectId = 1},
                    } },
                    new() { Id = 2, Name = "Project2", Description = "Description2", ProjectImages = new()
                    {
                        new(){ Id = 4, Path = @"c:\computer\test4.jpg", ProjectId = 2},
                        new(){ Id = 5, Path = @"c:\computer\test5.jpg", ProjectId = 2},
                        new(){ Id = 6, Path = @"c:\computer\test6.jpg", ProjectId = 2},
                    } }
                }
            };                                      
           
            _databaseContext.Users.AddRange(user);
            _databaseContext.SaveChanges();
        }

        [Theory]
        [InlineData(1, 3), InlineData(2, 3)]
        public async Task GetProjectImagesAsync_ForGivenProjectId_ShouldReturnProjectImages(int projectId, int expectedCount)
        {
            // Act
            List<ProjectImageDto> result = await _projectImageService.GetProjectImagesAsync(projectId);

            // Assert
            expectedCount.Should().Be(result.Count);
        }

        [Fact]
        public async Task GetProjectImagesAsync_ForNonExistingProject_ShouldReturnEmptyList()
        {
            //Arrange   
            int projectId = 99999;

            // Act
            List<ProjectImageDto> result = await _projectImageService.GetProjectImagesAsync(projectId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_ForGivenProjectImage_AddsProjectImageToDatabase()
        {
            //Arrange
            string path = ProjectDirectory.ProjectDirectoryFullPath + @"\HelpersForTesting\ProjectImage.jpg";
            
            //Arrange
            CreateProjectImageDto createProjectImageDto = new(1, path, 1);

            // Act
            int result = await _projectImageService.CreateAsync(createProjectImageDto);

            // Assert
            result.Should().Be(1);
        }
    }
}