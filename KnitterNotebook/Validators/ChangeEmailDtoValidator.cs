using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
    {
        private readonly DatabaseContext _databaseContext;

        public ChangeEmailDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Email)
                .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AllAsync(x => x.Email != value, cancellationToken))
                .WithMessage("E-mail jest już używany")
                .EmailAddress().WithMessage("Niepoprawny format e-mail");
        }
    }
}