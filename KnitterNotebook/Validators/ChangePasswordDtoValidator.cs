using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    private readonly IUserService _userService;

    public ChangePasswordDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(dto => dto.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage(Translations.UserNotFound);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(Translations.PasswordCantBeEmpty)
            .SetValidator(new PasswordValidator())
            .Equal(x => x.ConfirmPassword)
            .WithMessage(Translations.PasswordsAreNotIdentical);
    }
}