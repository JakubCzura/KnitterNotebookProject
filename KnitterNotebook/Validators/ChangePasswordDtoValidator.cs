using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        private readonly DatabaseContext _databaseContext;

        public ChangePasswordDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.NewPassword)
                .Length(6, 50).WithMessage("Hasło musi mieć 6-50 znaków")
                .Must(y => y is not null && y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
                .Must(y => y is not null && y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
                .Must(y => y is not null && y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę")
                .Equal(x => x.ConfirmPassword).WithMessage("Nowe hasło ma dwie różne wartości");
        }
    }
}