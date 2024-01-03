using FluentValidation;
using KnitterNotebook.Properties;
using System.Linq;

namespace KnitterNotebook.Validators;

public class NicknameValidator : AbstractValidator<string>
{
    public NicknameValidator() => RuleFor(nickname => nickname)
        .NotEmpty()
        .WithMessage(Translations.NicknameCantBeEmpty)
        .MaximumLength(50)
        .WithMessage($"{Translations.NicknameMaxChars} 50 {Translations.characters}")
        .Must(value => value.All(x => char.IsLetterOrDigit(x) || x == ' '))
        .WithMessage(Translations.NicknameCanConsistOfLettersSpacesAndNumbers);
}