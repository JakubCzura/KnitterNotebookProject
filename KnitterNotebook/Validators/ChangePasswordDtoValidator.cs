using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

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
                .NotEmpty().WithMessage("Wartość nie może być pusta")
                .SetValidator(new PasswordValidator())
                .Equal(x => x.ConfirmPassword).WithMessage("Nowe hasło ma dwie różne wartości");
        }
    }
}