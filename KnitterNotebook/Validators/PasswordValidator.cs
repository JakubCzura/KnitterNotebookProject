using FluentValidation;
using KnitterNotebook.Properties;
using System.Linq;

namespace KnitterNotebook.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator() => RuleFor(password => password)
        .NotNull()
        .WithMessage(Translations.PasswordCantBeEmpty)
        .Length(6, 50)
        .WithMessage($"{Translations.PasswordLengthMustBeBetween} 6 - 50 {Translations.characters}")
        .Must(y => y.Any(char.IsDigit))
        .WithMessage(Translations.PasswordMustHaveAtLeastOneNumber)
        .Must(y => y.Any(char.IsAsciiLetterLower))
        .WithMessage(Translations.PasswordMustHaveAtLeastOneLowerLetter)
        .Must(y => y.Any(char.IsAsciiLetterUpper))
        .WithMessage(Translations.PasswordMustHaveAtLeastOneUpperLetter);
}