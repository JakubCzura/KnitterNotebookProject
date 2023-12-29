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

public class SampleServiceTests
{
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly SampleService _sampleService;
    private readonly UserService _userService;
    private readonly Mock<IThemeService> _themeServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<SharedResourceViewModel> _sharedResourceViewModelMock = new();

    public SampleServiceTests()
    {
        _userService = new(_databaseContext, _themeServiceMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object, _configurationMock.Object, _sharedResourceViewModelMock.Object);
        _sampleService = new(_databaseContext, _userService);
        DatabaseHelper.CreateEmptyDatabase(_databaseContext);
        SeedData();
    }

    private void SeedData()
    {
        User user = new()
        {
            Email = "email@email.com",
            Nickname = "NicknameOfUserForTestingPurposes",
            Password = "Password123",
            ThemeId = 1,
            Samples =
            [
                new()
                {
                    YarnName = "yarn name",
                    LoopsQuantity = 10,
                    RowsQuantity = 6,
                    NeedleSize = 3.5,
                    NeedleSizeUnit = NeedleSizeUnit.mm,
                    Description = "sample description"
                },
                new()
                {
                    YarnName = "yarn name 2",
                    LoopsQuantity = 5,
                    RowsQuantity = 6,
                    NeedleSize = 2.5,
                    NeedleSizeUnit = NeedleSizeUnit.cm,
                    Image = new(@"c:\computer\file.jpg")
                },
                new()
                {
                    YarnName = "yarn name 3",
                    LoopsQuantity = 2,
                    RowsQuantity = 2,
                    NeedleSize = 2,
                    NeedleSizeUnit = NeedleSizeUnit.mm
                }
            ]
        };

        _databaseContext.Users.Add(user);
        _databaseContext.SaveChanges();
    }

    [Fact]
    public async Task CreateAsync_ForValidDataWithoutPhoto_CreatesNewSample()
    {
        //Assert
        CreateSampleDto createSampleDto = new("yarn name 4", 10, 6, 3.5, NeedleSizeUnit.mm, "sample description 4", 1, null);

        //Act
        int result = await _sampleService.CreateAsync(createSampleDto);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ForValidData_CreatesNewSample()
    {
        //Assert
        string path = Path.Combine(Paths.ProjectDirectory, "HelpersForTesting", "ProjectImage.jpg");
        CreateSampleDto createSampleDto = new("yarn name 4", 10, 6, 3.5, NeedleSizeUnit.mm, "sample description 4", 1, path);

        //Act
        int result = await _sampleService.CreateAsync(createSampleDto);

        //Assert
        result.Should().Be(2);
        Directory.GetFiles(Paths.UserDirectory("NicknameOfUserForTestingPurposes")).Any(x => x.EndsWith("ProjectImage.jpg")).Should().BeTrue();

        //Clean up after test
        Directory.Delete(Paths.UserDirectory("NicknameOfUserForTestingPurposes"), true);
    }

    [Fact]
    public async Task CreateAsync_ForNotExistingUser_ThrowsEntityNotFoundException()
    {
        //Assert
        CreateSampleDto createSampleDto = new("yarn name 4", 10, 6, 3.5, NeedleSizeUnit.mm, "sample description 4", 99999, null);

        //Act
        Func<Task> act = async () => await _sampleService.CreateAsync(createSampleDto);

        //Assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ForNullData_ThrowsNullReferenceException()
    {
        //Assert
        CreateSampleDto createSampleDto = null!;

        //Act
        Func<Task> act = async () => await _sampleService.CreateAsync(createSampleDto);

        //Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task GetUserSamplesAsync_ForExistingUser_ReturnsUserSamples()
    {
        //Arrange
        int userId = 1;

        //Act
        List<SampleDto> result = await _sampleService.GetUserSamplesAsync(userId);

        //Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetUserSamplesAsync_ForNotExistingUser_ReturnsEmptyList()
    {
        //Arrange
        int userId = 99999;

        //Act
        List<SampleDto> result = await _sampleService.GetUserSamplesAsync(userId);

        //Assert
        result.Should().BeEmpty();
    }
}