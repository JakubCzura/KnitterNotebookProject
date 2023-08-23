using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class ChangeNicknameDtoValidator : AbstractValidator<ChangeNicknameDto>
    {
        private readonly IUserService _userService;

        public ChangeNicknameDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _userService.UserExists(id))
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Nickname)
                .NotNull().WithMessage("Wartość nie może być pusta")
                .SetValidator(new NicknameValidator())
                .MustAsync(async (nickname, cancellationToken) => !await _userService.IsNicknameTaken(nickname))
                .WithMessage("Nick jest już używany");
        }
    }
}