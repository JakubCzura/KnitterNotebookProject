using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class ChangeThemeDtoValidator : AbstractValidator<ChangeThemeDto>
    {
        private readonly DatabaseContext _databaseContext;

        public ChangeThemeDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(dto => dto.UserId)
               .MustAsync(async (id, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
               .WithMessage("Nie znaleziono użytkownika");

            RuleFor(dto => dto.ThemeName)
              .MustAsync(async (value, cancellationToken) => await _databaseContext.Themes.AnyAsync(x => x.Name == value, cancellationToken))
              .WithMessage("Nie znaleziono podanego motywu w bazie danych");
        }
    }
}