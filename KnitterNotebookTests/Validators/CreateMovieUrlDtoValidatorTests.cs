using FluentValidation;
using FluentValidation.TestHelper;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
using KnitterNotebookTests.HelpersForTesting;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebookTests.Validators
{
    public class CreateMovieUrlDtoValidatorTests
    {
        private readonly IValidator<CreateMovieUrlDto> _validator;
        private readonly DatabaseContext _databaseContext;

        public CreateMovieUrlDtoValidatorTests()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new();
            builder.UseInMemoryDatabase(DatabaseHelper.CreateUniqueDatabaseName);
            _databaseContext = new DatabaseContext(builder.Options);
            _validator = new CreateMovieUrlDtoValidator(_databaseContext);
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new()
            {
                new User() { Id = 1 },
                new User() { Id = 2 }
            };
            _databaseContext.Users.AddRange(users);
            _databaseContext.SaveChanges();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new CreateMovieUrlDto(null!, "https://youtube.pl", 1) };
            yield return new object[] { new CreateMovieUrlDto("Movie", null!, 1) };
            yield return new object[] { new CreateMovieUrlDto("Funny movie", "https://urltestmovie/96.pl", -1) };
            yield return new object[] { new CreateMovieUrlDto(string.Empty, "https://testurltestmovie/96.pl", 2) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, 2) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, 0) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, 3) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateMovieUrlDto("Title", "https://youtube.pl", 1) };
            yield return new object[] { new CreateMovieUrlDto("Movie", "https://movieurl/21.pl", 2) };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ValidateAsync_ForInvalidData_FailValidation(CreateMovieUrlDto createMovieUrlDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(createMovieUrlDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ValidateAsync_ForValidData_PassValidation(CreateMovieUrlDto createMovieUrlDto)
        {
            //Act
            var validationResult = await _validator.TestValidateAsync(createMovieUrlDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}