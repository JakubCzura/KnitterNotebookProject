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
              .MustAsync(async (id, cancellationToken) => await databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
              .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Nickname)
               .NotNull().WithMessage("Wartość nie może być pusta")
               .CustomAsync(async (value, context, cancellationToken) =>
               {
                   bool isNicknameUsed = await _databaseContext.Users.AnyAsync(x => x.Nickname == value, cancellationToken);
                   if (isNicknameUsed)
                       context.AddFailure(nameof(RegisterUserDto.Nickname), "Nick jest już używany");

                   if (value is not null && !value.All(x => char.IsLetterOrDigit(x)))
                       context.AddFailure(nameof(RegisterUserDto.Nickname), "Nickname może zawierać tylko litery i cyfry");
               })
              .MinimumLength(1).WithMessage("Nazwa użytkownika musi mieć conajmniej 1 znak")
              .MaximumLength(50).WithMessage("Nazwa użytkownika może mieć maksimum 50 znaków"); ;
        }
    }
}