using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    private readonly IUserService _userService;

    public ResetPasswordDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.NewPassword)
          .SetValidator(new PasswordValidator())
          .Equal(x => x.RepeatedNewPassword)
          .WithMessage(Translations.PasswordsAreNotIdentical);

        RuleFor(x => x.Email)
          .MustAsync(async (email, cancellationToken) => await _userService.IsEmailTakenAsync(email))
          .WithMessage(Translations.EmailNotFound);

        RuleFor(x => x.Token)
          .MustAsync(async (token, cancellationToken) => await _userService.ArePasswordResetTokenAndExpirationDateValidAsync(token))
          .WithMessage(Translations.TokenInvalidOrExpired);
    }
}