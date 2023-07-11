using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class ChangeNicknameDtoValidator : AbstractValidator<ChangeNicknameDto>
    {
        private readonly DatabaseContext _databaseContext;

        public ChangeNicknameDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Nickname)
                .NotNull().WithMessage("Wartość nie może być pusta")
                .SetValidator(new NicknameValidator())
                .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AllAsync(x => x.Nickname != value, cancellationToken))
                .WithMessage("Nick jest już używany");
        }
    }
}