using FluentValidation;
using System.Linq;

namespace KnitterNotebook.Validators;

public class NicknameValidator : AbstractValidator<string>
{
    public NicknameValidator()
    {
        RuleFor(nickname => nickname)
            .Length(1, 50).WithMessage("Nazwa użytkownika musi mieć 1-50 znaków")
            .NotEmpty().WithMessage("Nazwa użytkownika nie może być pusta")
            .Must(value => value.All(x => char.IsLetterOrDigit(x) || x == ' ')).WithMessage("Nickname może zawierać tylko litery, spacje i cyfry");
    }
}