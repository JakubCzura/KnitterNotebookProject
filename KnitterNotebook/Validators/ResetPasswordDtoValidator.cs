using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        private readonly DatabaseContext _databaseContext;

        public ResetPasswordDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(x => x.NewPassword)
              .NotEmpty().WithMessage("Hasło nie może być puste")
              .MinimumLength(6).WithMessage("Hasło musi mieć conajmniej 6 znaków")
              .MaximumLength(50).WithMessage("Hasło może mieć maksimum 50 znaków")
              .Must(y => y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
              .Must(y => y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
              .Must(y => y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę")
              .Equal(x => x.RepeatedNewPassword).WithMessage("Nowe hasło ma dwie różne wartości");

            RuleFor(x => x.EmailOrNickname)
              .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Nickname == value || x.Email == value, cancellationToken))
              .WithMessage("E-mail oraz nazwa użytkownika nie pasują do żadnego użytkownika");
        }
    }
}