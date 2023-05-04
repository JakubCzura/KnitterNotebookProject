using FluentValidation;
using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Validators
{
    public class CreateMovieUrlValidator : AbstractValidator<CreateMovieUrl>
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
