using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private readonly IUserService _userService;

    public RegisterUserDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Nickname)
            .SetValidator(new NicknameValidator())
            .MustAsync(async (nickname, cancellationToken) => !await _userService.IsNicknameTakenAsync(nickname))
            .WithMessage(Translations.NicknameIsAlreadyTaken);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Translations.EmailCantBeEmpty)
            .EmailAddress().WithMessage(Translations.InvalidEmailFormat)
            .MustAsync(async (email, cancellationToken) => !await _userService.IsEmailTakenAsync(email))
            .WithMessage(Translations.EmailIsAlreadyTaken);

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());
    }
}