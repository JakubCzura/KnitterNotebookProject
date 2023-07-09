using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly DatabaseContext _databaseContext;

        public RegisterUserDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(x => x.Nickname)
                .NotEmpty().WithMessage("Nazwa użytkownika nie może być pusta")
                .MinimumLength(1).WithMessage("Nazwa użytkownika musi mieć conajmniej 1 znak")
                .MaximumLength(50).WithMessage("Nazwa użytkownika może mieć maksimum 50 znaków")
                .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AllAsync(x => x.Nickname != value, cancellationToken))
                .WithMessage("Nick jest już używany");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail nie może być pusty")
                .MaximumLength(50).WithMessage("E-mail może mieć maksimum 50 znaków")
                .EmailAddress().WithMessage("Niepoprawny format e-mail")
                .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AllAsync(x => x.Email != value, cancellationToken))
                .WithMessage("E-mail jest już używany");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło nie może być puste")
                .MinimumLength(6).WithMessage("Hasło musi mieć conajmniej 6 znaków")
                .MaximumLength(50).WithMessage("Hasło może mieć maksimum 50 znaków")
                .Must(y => y is not null && y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
                .Must(y => y is not null && y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
                .Must(y => y is not null && y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę");
        }
    }
}