using FluentValidation;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Validators
{
    public class LogInDtoValidator : AbstractValidator<LogInDto>
    {
        public LogInDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email nie ma poprawnej formy");

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());
        }
    }
}