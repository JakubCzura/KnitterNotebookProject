using FluentValidation;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Validators
{
    public class LogInDtoValidator : AbstractValidator<LogInDto>
    {
        public LogInDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Wartość nie może być pusta")
                .EmailAddress().WithMessage("Email nie ma poprawnej formy");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Wartość nie może być pusta")
                .SetValidator(new PasswordValidator());
        }
    }
}