using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private readonly IUserService _userService;

    public RegisterUserDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Nickname)
            .NotEmpty().WithMessage("Nazwa użytkownika nie może być pusta")
            .SetValidator(new NicknameValidator())
            .MustAsync(async (nickname, cancellationToken) => !await _userService.IsNicknameTakenAsync(nickname))
            .WithMessage("Nick jest już używany");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail nie może być pusty")
            .EmailAddress().WithMessage("Niepoprawny format e-mail")
            .MustAsync(async (email, cancellationToken) => !await _userService.IsEmailTakenAsync(email))
            .WithMessage("E-mail jest już używany");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Hasło nie może być puste")
            .SetValidator(new PasswordValidator());
    }
}