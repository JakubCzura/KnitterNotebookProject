using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

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
                .SetValidator(new NicknameValidator())
                .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AllAsync(x => x.Nickname != value, cancellationToken))
                .WithMessage("Nick jest już używany");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail nie może być pusty")
                .EmailAddress().WithMessage("Niepoprawny format e-mail")
                .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AllAsync(x => x.Email != value, cancellationToken))
                .WithMessage("E-mail jest już używany");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło nie może być puste")
                .SetValidator(new PasswordValidator());
        }
    }
}