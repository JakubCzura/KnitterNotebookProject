using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services;

namespace KnitterNotebook.IntegrationTests.Services;

public class CrudServiceTests : IDisposable
{
    private readonly DatabaseContext _databaseContext = DatabaseHelper.CreateDatabaseContext();
    private readonly CrudService<User> _crudService;

    public CrudServiceTests()
    {
        _databaseContext.Database.EnsureCreated();
        _crudService = new(_databaseContext);
        SeedUsers();
    }

    public void Dispose()
    {
        _databaseContext.Database.EnsureDeleted();
        _databaseContext.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SeedUsers()
    {
        List<Theme> themes =
        [
            new() { Name = ApplicationTheme.Default },
            new() { Name = ApplicationTheme.Light },
            new() { Name = ApplicationTheme.Dark }
        ];
        _databaseContext.Themes.AddRange(themes);

        List<User> users =
        [
            new() { Email = "test1@test.com", Nickname = "nickname1", Password = "Password1231", ThemeId = 1 },
            new() { Email = "test2@test.com", Nickname = "nickname2", Password = "Password1232", ThemeId = 1 },
            new() { Email = "test3@test.com", Nickname = "nickname3", Password = "Password1233", ThemeId = 2 },
            new() { Email = "test4@test.com", Nickname = "nickname4", Password = "Password1234", ThemeId = 3 },
        ];
        _databaseContext.Users.AddRange(users);
        _databaseContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllRows()
    {
        //Act
        IEnumerable<User> result = await _crudService.GetAllAsync();

        //Assert
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetAsync_ForExistingRow_ReturnsRow()
    {
        //Act
        User? result = await _crudService.GetAsync(1);

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAsync_ForNotExistingRow_ReturnsNull()
    {
        //Act
        User? result = await _crudService.GetAsync(99999999);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ForValidData_AddsRowToDatabase()
    {
        //Arrange
        User user = new() { Email = "test1@test.com", Nickname = "nickname1", Password = "Password123!@#", ThemeId = 1 };

        //Act
        int result = await _crudService.CreateAsync(user);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ForInvalidData_DoesNotAddRowToDatabase()
    {
        //Arrange
        User user = null!;

        //Act
        int result = await _crudService.CreateAsync(user);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task UpdateAsync_ForValidData_UpdatesRowInDatabase()
    {
        //Arrange
        User user = _databaseContext.Users.Find(1)!;
        user.Email = "new_test1@test.com";

        //Act
        int result = await _crudService.UpdateAsync(user);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ForInvalidData_DoesNotUpdateRowInDatabase()
    {
        //Arrange
        User user = null!;

        //Act
        int result = await _crudService.UpdateAsync(user);

        //Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task DeleteAsync_ForExistingRow_DeletesRowFromDatabase()
    {
        //Act
        int result = await _crudService.DeleteAsync(1);

        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task DeleteAsync_ForNotExistingRow_DoesNotDeleteRowFromDatabase()
    {
        //Act
        int result = await _crudService.DeleteAsync(99999999);

        //Assert
        result.Should().Be(0);
    }
}