using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
    {
        private readonly IUserService _userService;

        public ChangeEmailDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _userService.UserExists(id))
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellationToken) => !await _userService.IsEmailTaken(email))
                .WithMessage("E-mail jest już używany")
                .EmailAddress().WithMessage("Niepoprawny format e-mail");
        }
    }
}