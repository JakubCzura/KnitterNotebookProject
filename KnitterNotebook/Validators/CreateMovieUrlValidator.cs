using FluentValidation;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Validators
{
    public class CreateMovieUrlValidator : AbstractValidator<CreateMovieUrlDto>
    {
        public CreateMovieUrlValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tytuł nie może być pusty");

            RuleFor(x => x.Link)
                .NotEmpty().WithMessage("Link do filmu nie może być pusty");

            RuleFor(x => x.User)
                .NotNull().WithMessage("Użytkownik musi być podany przy dodawaniu filmu");
        }
    }
}