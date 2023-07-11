using FluentValidation;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class NicknameValidator : AbstractValidator<string>
    {
        public NicknameValidator()
        {
            RuleFor(nickname => nickname)
                .Length(1, 50).WithMessage("Nazwa użytkownika musi mieć 1-50 znaków")
                .Must(value => value.All(x => char.IsLetterOrDigit(x))).WithMessage("Nickname może zawierać tylko litery i cyfry");
        }
    }
}