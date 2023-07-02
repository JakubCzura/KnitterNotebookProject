using FluentValidation;
using FluentValidation.TestHelper;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;

namespace KnitterNotebookTests.Validators
{
    public class CreateMovieUrlValidatorTests
    {
        private readonly IValidator<CreateMovieUrlDto> _validator;

        public CreateMovieUrlValidatorTests()
        {
            _validator = new CreateMovieUrlValidator();
        }

        public static IEnumerable<object[]> InvalidData()
        {
            yield return new object[] { new CreateMovieUrlDto(null!, "https://youtube.pl", new User()) };
            yield return new object[] { new CreateMovieUrlDto("Movie", null!, new User()) };
            yield return new object[] { new CreateMovieUrlDto("Funny movie", "https://urltestmovie/96.pl", null!) };
            yield return new object[] { new CreateMovieUrlDto(string.Empty, "https://testurltestmovie/96.pl", new User()) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, new User()) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", string.Empty, null!) };
        }

        public static IEnumerable<object[]> ValidData()
        {
            yield return new object[] { new CreateMovieUrlDto("Title", "https://youtube.pl", new User()) };
            yield return new object[] { new CreateMovieUrlDto("Movie", "https://movieurl/21.pl", new User()) };
            yield return new object[] { new CreateMovieUrlDto("Funny movie", "https://urltestmovie/96.pl", new User()) };
            yield return new object[] { new CreateMovieUrlDto("Funny movie2", "https://testurltestmovie/96.pl", new User()) };
            yield return new object[] { new CreateMovieUrlDto("Scary movie", "https://testurltestmovie/1996.pl", new User()) };
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public void Validate_ForInvalidData_FailValidation(CreateMovieUrlDto createMovieUrlDto)
        {
            //Act
            var validationResult = _validator.TestValidate(createMovieUrlDto);

            //Assert
            validationResult.ShouldHaveAnyValidationError();
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void Validate_ForValidData_PassValidation(CreateMovieUrlDto createMovieUrlDto)
        {
            //Act
            var validationResult = _validator.TestValidate(createMovieUrlDto);

            //Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}