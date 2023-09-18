using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ChangeNicknameDtoValidator : AbstractValidator<ChangeNicknameDto>
{
    private readonly IUserService _userService;

    public ChangeNicknameDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(dto => dto.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage("Nie znaleziono użytkownika");

        RuleFor(x => x.Nickname)
            .NotNull().WithMessage("Wartość nie może być pusta")
            .SetValidator(new NicknameValidator())
            .MustAsync(async (nickname, cancellationToken) => !await _userService.IsNicknameTakenAsync(nickname))
            .WithMessage("Nick jest już używany");
    }
}