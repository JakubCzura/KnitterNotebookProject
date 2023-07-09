using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class CreateMovieUrlDtoValidator : AbstractValidator<CreateMovieUrlDto>
    {
        private readonly DatabaseContext _databaseContext;

        public CreateMovieUrlDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tytuł nie może być pusty");

            RuleFor(x => x.Link)
                .NotEmpty().WithMessage("Link do filmu nie może być pusty");

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
                .WithMessage("Nie znaleziono użytkownika");
        }
    }
}