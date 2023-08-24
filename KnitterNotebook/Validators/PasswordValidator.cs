using FluentValidation;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(password => password)
                .NotNull().WithMessage("Wartość nie może być pusta")
                .Length(6, 50).WithMessage("Hasło musi mieć 6-50 znaków")
                .Must(y => y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
                .Must(y => y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
                .Must(y => y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę");
        }
    }
}