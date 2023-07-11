using FluentValidation;
using KnitterNotebook.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Validators
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(password => password)
                .Length(6, 50).WithMessage("Hasło musi mieć 6-50 znaków")
                .Must(y => y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
                .Must(y => y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
                .Must(y => y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę");
        }
    }
}
