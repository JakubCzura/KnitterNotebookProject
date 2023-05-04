using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using OneOf.Types;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        private readonly IUserService _userService;

        public ChangePasswordDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(dto => dto.UserId)
                .Must(id => _userService.GetAsync(id) != null)
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.NewPassword)
              .NotEmpty().WithMessage("Hasło nie może być puste")
              .MinimumLength(6).WithMessage("Hasło musi mieć conajmniej 6 znaków")
              .MaximumLength(50).WithMessage("Hasło może mieć maksimum 50 znaków")
              .Must(y => y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
              .Must(y => y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
              .Must(y => y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę")
              .Equal(x => x.ConfirmPassword).WithMessage("Nowe hasło ma dwie różne wartości");         
        }
    }
}