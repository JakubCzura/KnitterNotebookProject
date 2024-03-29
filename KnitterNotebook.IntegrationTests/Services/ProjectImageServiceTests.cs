﻿using FluentAssertions;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KnitterNotebook.IntegrationTests.Services;

public class ProjectImageServiceTests : IDisposable
{
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly ProjectImageService _projectImageService;
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public ProjectImageServiceTests()
    {
        _databaseContext.Database.EnsureCreated();
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object, _sharedResourceViewModelMock.Object);
        _projectImageService = new(_databaseContext, _userService);
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
            Email = "test@test.com",
            Nickname = "NicknameOfUserForTestingPurposes",
            Password = "Password123",
            Projects =
            [
                new()
                {
                    Name = "Project1",
                    Description = "Description1",
                    ProjectImages =
                [
                    new() { Path = @"c:\computer\test1.jpg" },
                    new() { Path = @"c:\computer\test2.jpg" },
                    new() { Path = @"c:\computer\test3.jpg" },
                ]
                },
                new()
                {
                    Name = "Project2",
                    Description = "Description2",
                    ProjectImages =
                [
                    new() { Path = @"c:\computer\test4.jpg" },
                    new() { Path = @"c:\computer\test5.jpg" },
                    new() { Path = @"c:\computer\test6.jpg" },
                ]
                }
            ],
            ThemeId = 1
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
        string path = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "ProjectImage.jpg");
        CreateProjectImageDto createProjectImageDto = new(1, path, 1);

        // Act
        int result = await _projectImageService.CreateAsync(createProjectImageDto);

        // Assert
        result.Should().Be(1);
        Directory.GetFiles(Paths.UserDirectory("NicknameOfUserForTestingPurposes")).Any(x => x.EndsWith("ProjectImage.jpg")).Should().BeTrue();

        //Clean up after test
        Directory.Delete(Paths.UserDirectory("NicknameOfUserForTestingPurposes"), true);
    }

    [Fact]
    public async Task CreateAsync_ForNullProjectImage_ThrowNullReferenceException()
    {
        //Arrange
        CreateProjectImageDto createProjectImageDto = null!;

        // Act
        Func<Task> action = async () => await _projectImageService.CreateAsync(createProjectImageDto);

        // Assert
        await action.Should().ThrowAsync<NullReferenceException>();
    }
}