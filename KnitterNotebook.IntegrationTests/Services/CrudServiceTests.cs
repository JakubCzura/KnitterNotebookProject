using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class CrudServiceTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly CrudService _crudService;

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
                new User() { Id = 1, Email = "test1@test.com", Nickname = "nickname1"},
                new User() { Id = 2, Email = "test2@test.com", Nickname = "nickname2"},
                new User() { Id = 3, Email = "test3@test.com", Nickname = "nickname3"},
                new User() { Id = 4, Email = "test4@test.com", Nickname = "nickname4"},
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllRows()
        {
            //Act
            var result = await _crudService.GetAllAsync();

            //Assert
            result.Should().HaveCount(4);
        }

        [Fact]
        public async Task GetAsync_ForExistingRow_ReturnsRow()
        {
            //Act
            var result = await _crudService.GetAsync(1);

            //Assert
            result.Should().NotBeNull()
        }

        [Fact]
        public async Task GetAsync_ForNotExistingRow_ReturnsNull()
        {
            //Act
            var result = await _crudService.GetAsync(99999999);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ForValidData_AddsRowToDatabase()
        {
            //Arrange
            User user = new() {Email = "test1@test.com", Nickname = "nickname1", Password = "Password123!@#", ThemeId = 1};

            //Act
            var result = await _crudService.CreateAsync(user);

            //Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task UpdateAsync_ForValidData_UpdatesRowInDatabase()
        {
            //Arrange
            User user = new() {Id = 1, Email = "new_test1@test.com", Nickname = "new_nickname1", Password = "NewPassword123!@#", ThemeId = 1};

            //Act
            var result = await _crudService.UpdateAsync(user);

            //Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task DeleteAsync_ForValidId_DeletesRowFromDatabase()
        {
            //Act
            var result = await _crudService.DeleteAsync(1);

            //Assert
            result.Should().Be(1);
        }
    }
}