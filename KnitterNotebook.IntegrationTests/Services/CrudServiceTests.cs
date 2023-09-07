using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class CrudServiceTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly CrudService<User> _crudService;

        public CrudServiceTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _crudService = new(_databaseContext);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new() { Id = 1, Email = "test1@test.com", Nickname = "nickname1" },
                new() { Id = 2, Email = "test2@test.com", Nickname = "nickname2" },
                new() { Id = 3, Email = "test3@test.com", Nickname = "nickname3" },
                new() { Id = 4, Email = "test4@test.com", Nickname = "nickname4" },
            };
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
    }
}