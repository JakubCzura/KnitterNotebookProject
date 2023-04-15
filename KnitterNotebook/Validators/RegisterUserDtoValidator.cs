using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly KnitterNotebookContext _knitterNotebookContext;

        public RegisterUserDtoValidator(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;

            RuleFor(x => x.Nickname)
                .NotEmpty().WithMessage("Nazwa użytkownika nie może być pusta")
                .MinimumLength(1).WithMessage("Nazwa użytkownika musi mieć conajmniej 1 znak")
                .MaximumLength(50).WithMessage("Nazwa użytkownika może mieć maksimum 50 znaków");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail nie może być pusty")
                .MaximumLength(50).WithMessage("E-mail może mieć maksimum 50 znaków")
                .EmailAddress().WithMessage("Niepoprawny format e-mail");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło nie może być puste")
                .MinimumLength(6).WithMessage("Hasło musi mieć conajmniej 6 znaków")
                .MaximumLength(50).WithMessage("Hasło może mieć maksimum 50 znaków")
                .Must(y => y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
                .Must(y => y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
                .Must(y => y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę");

            RuleFor(x => x.Nickname)
               .Custom((value, context) =>
               {
                   bool isNicknameUsed = _knitterNotebookContext.Users.Any(x => x.Nickname == value);
                   if (isNicknameUsed)
                   {
                       context.AddFailure(nameof(RegisterUserDto.Nickname), "Nick jest już używany");
                   }
               });

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    bool isEmailUsed = _knitterNotebookContext.Users.Any(x => x.Email == value);
                    if (isEmailUsed)
                    {
                        context.AddFailure(nameof(RegisterUserDto.Email), "E-mail jest już używany");
                    }
                });
        }
    }
}