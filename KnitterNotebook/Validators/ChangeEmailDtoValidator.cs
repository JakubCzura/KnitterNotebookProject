using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
{
    private readonly IUserService _userService;

    public ChangeEmailDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(dto => dto.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage(Translations.UserNotFound);

        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage(Translations.EmailCantBeEmpty)
            .MustAsync(async (email, cancellationToken) => !await _userService.IsEmailTakenAsync(email))
            .WithMessage(Translations.EmailIsAlreadyTaken)
            .EmailAddress()
            .WithMessage(Translations.InvalidEmailFormat);
    }
}