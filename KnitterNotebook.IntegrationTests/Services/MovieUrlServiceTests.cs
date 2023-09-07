using FluentAssertions;
using KnitterNotebook.Database;
using KnitterNotebook.IntegrationTests.HelpersForTesting;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.IntegrationTests.Services
{
    public class MovieUrlServiceTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly MovieUrlService _movieUrlService;

        public MovieUrlServiceTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _movieUrlService = new(_databaseContext);
            SeedMovieUrls();
        }

        private void SeedMovieUrls()
        {
            List<MovieUrl> movieUrls = new()
            {
                new() { Id = 1, Title = "sample title 1", Link = new("http://testlink1.com"), Description = null, UserId = 1 },
                new() { Id = 2, Title = "sample title 2", Link = new("http://testlink2.com"), Description = "description 2", UserId = 1 },
                new() { Id = 3, Title = "sample title 3", Link = new("http://testlink3.com"), Description = null, UserId = 2 },
                new() { Id = 4, Title = "sample title 4", Link = new("http://testlink4.com"), Description = null, UserId = 2 },
                new() { Id = 5, Title = "sample title 5", Link = new("http://testlink5.com"), Description = "description 4", UserId = 2 }
            };
            _databaseContext.MovieUrls.AddRange(movieUrls);
            _databaseContext.SaveChanges();
        }

        [Fact]
        public async Task CreateAsync_ForValidData_CreatesNewMovieUrl()
        {
            //Assert
            CreateMovieUrlDto createMovieUrl = new("sample title 6", "http://testlink6.com", "description 6", 1);

            //Act
            int result = await _movieUrlService.CreateAsync(createMovieUrl);

            //Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task CreateAsync_ForInvalidLink_ThrowsUriFormatException()
        {
            //Assert
            CreateMovieUrlDto createMovieUrl = new("sample title 6", "testlink6.com", "description 6", 1);

            //Act
            Func<Task<int>> action = async () => await _movieUrlService.CreateAsync(createMovieUrl);

            //Assert
            await action.Should().ThrowAsync<UriFormatException>();
        }

        [Fact]
        public async Task CreateAsync_ForNullData_ThrowsNullReferenceException()
        {
            //Assert
            CreateMovieUrlDto createMovieUrl = null!;

            //Act
            Func<Task<int>> action = async () => await _movieUrlService.CreateAsync(createMovieUrl);

            //Assert
            await action.Should().ThrowAsync<NullReferenceException>();
        }

        [Theory]
        [InlineData(1, 2), InlineData(2, 3)]
        public async Task GetUserMovieUrlsAsync_ForExistingUser_ReturnsUserMovieUrls(int userId, int expectedCount)
        {
            //Act
            List<MovieUrlDto> result = await _movieUrlService.GetUserMovieUrlsAsync(userId);

            //Assert
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task GetUserMovieUrlsAsync_ForNotExistingUser_ReturnsEmptyList()
        {
            //Assert
            int userId = 999;

            //Act
            List<MovieUrlDto> result = await _movieUrlService.GetUserMovieUrlsAsync(userId);

            //Assert
            result.Should().BeEmpty();
        }
    }
}