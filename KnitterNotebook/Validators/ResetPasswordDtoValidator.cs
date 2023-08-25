using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        private readonly IUserService _userService;

        public ResetPasswordDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.NewPassword)
              .NotEmpty().WithMessage("Hasło nie może być puste")
              .SetValidator(new PasswordValidator())
              .Equal(x => x.RepeatedNewPassword).WithMessage("Nowe hasło ma dwie różne wartości");

            RuleFor(x => x.Email)
              .MustAsync(async (email, cancellationToken) => await _userService.IsEmailTakenAsync(email))
              .WithMessage("E-mail nie pasuje do żadnego użytkownika");
        }
    }
}