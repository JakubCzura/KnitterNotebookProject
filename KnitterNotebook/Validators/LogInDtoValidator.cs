using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;

namespace KnitterNotebook.Validators;

public class LogInDtoValidator : AbstractValidator<LogInDto>
{
    public LogInDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage(Translations.EmailCantBeEmpty)
            .EmailAddress()
            .WithMessage(Translations.InvalidEmailFormat);

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage(Translations.PasswordCantBeEmpty)
            .SetValidator(new PasswordValidator());
    }
}