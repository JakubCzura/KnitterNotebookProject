using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

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
              .SetValidator(new PasswordValidator())
              .Equal(x => x.RepeatedNewPassword).WithMessage("Nowe hasło ma dwie różne wartości");

            RuleFor(x => x.EmailOrNickname)
              .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Nickname == value || x.Email == value, cancellationToken))
              .WithMessage("E-mail oraz nazwa użytkownika nie pasują do żadnego użytkownika");
        }
    }
}