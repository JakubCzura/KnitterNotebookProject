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
    public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
    {
        private readonly DatabaseContext _databaseContext;

        public ChangeEmailDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Email)
                .CustomAsync(async (value, context, cancellationToken) =>
                {
                    bool isEmailUsed = await _databaseContext.Users.AnyAsync(x => x.Email == value, cancellationToken);
                    if (isEmailUsed)
                    {
                        context.AddFailure(nameof(RegisterUserDto.Email), "E-mail jest już używany");
                    }
                })
                .MaximumLength(50).WithMessage("E-mail może mieć maksimum 50 znaków")
                .EmailAddress().WithMessage("Niepoprawny format e-mail"); ;
        }
    }
}