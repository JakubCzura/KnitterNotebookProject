using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class UserServiceTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ThemeService _themeService;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            Dictionary<string, string> myConfiguration = new() { { "Tokens:ResetPasswordTokenExpirationDays", "1" } };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration!)
                .Build();

            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);

            _themeService = new(_databaseContext);
            _passwordService = new();
            _tokenService = new();
            _userService = new(_databaseContext, _themeService, _passwordService, _tokenService, configuration);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new() 
                { 
                    Id = 1,
                    Email = "user1@mail.com", 
                    Nickname = "Nickname1", 
                    Password = "Password123", 
                    PasswordResetToken = "PasswordResetToken1",
                    PasswordResetTokenExpirationDate = DateTime.UtcNow.AddDays(1),
                    ThemeId = 1 
                },
                new() 
                {
                    Id = 2,
                    Email = "user2@mail.com", 
                    Nickname = "Nickname2", 
                    Password = "User2Password123",
                    PasswordResetToken = "PasswordResetToken2",
                    PasswordResetTokenExpirationDate = DateTime.UtcNow.AddDays(1),
                    ThemeId = 1 
                },
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        [Fact]
        public async Task IsNicknameTakenAsync_ForExistingNickname_ReturnsTrue()
        {
            //Arrange
            string nickname = "Nickname1";

            //Act
            bool result = await _userService.IsNicknameTakenAsync(nickname);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsNicknameTakenAsync_ForNotExistingNickname_ReturnsFalse()
        {
            //Arrange
            string nickname = "Nickname3323112313213";

            //Act
            bool result = await _userService.IsNicknameTakenAsync(nickname);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsEmailTakenAsync_ForExistingEmail_ReturnsTrue()
        {
            //Arrange
            string email = "user1@mail.com";

            //Act
            bool result = await _userService.IsEmailTakenAsync(email);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsEmailTakenAsync_ForNotExistingEmail_ReturnsFalse()
        {
            //Arrange
            string email = "tes222t@dsadsadas321321312321mail.com";

            //Act
            bool result = await _userService.IsEmailTakenAsync(email);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ArePasswordResetTokenAndExpirationDateValidAsync_ForValidToken_ReturnsTrue()
        {
            //Arrange
            string token = "PasswordResetToken1";

            //Act
            bool result = await _userService.ArePasswordResetTokenAndExpirationDateValidAsync(token);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ArePasswordResetTokenAndExpirationDateValidAsync_ForInvalidToken_ReturnsFalse()
        {
            //Arrange
            string token = "TokenNotInDatabase132132131212121231213321";

            //Act
            bool result = await _userService.ArePasswordResetTokenAndExpirationDateValidAsync(token);

            //Assert
            result.Should().BeFalse();
        }
    }
}