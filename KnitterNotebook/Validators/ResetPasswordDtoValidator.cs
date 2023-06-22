using FluentValidation;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Validators
{
    internal class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        private readonly IUserService _userService;

        public ResetPasswordDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.NewPassword)
              .NotEmpty().WithMessage("Hasło nie może być puste")
              .MinimumLength(6).WithMessage("Hasło musi mieć conajmniej 6 znaków")
              .MaximumLength(50).WithMessage("Hasło może mieć maksimum 50 znaków")
              .Must(y => y.Any(char.IsDigit)).WithMessage("Hasło musi zawierać conajmniej jedną cyfrę")
              .Must(y => y.Any(char.IsAsciiLetterLower)).WithMessage("Hasło musi zawierać conajmniej jedną małą literę")
              .Must(y => y.Any(char.IsAsciiLetterUpper)).WithMessage("Hasło musi zawierać conajmniej jedną wielką literę")
              .Equal(x => x.RepeatedNewPassword).WithMessage("Nowe hasło ma dwie różne wartości");

            RuleFor(x => x.EmailOrNickname)
                .CustomAsync(async (value, context, cancellationToken) =>
                {
                    IEnumerable<User> users = await _userService.GetAllAsync();
                    bool userWithEmailOrNicknameExists = users.Any(x => x.Nickname == value || x.Email == value);
                    if (!userWithEmailOrNicknameExists)
                    {
                        context.AddFailure(nameof(ResetPasswordDto.EmailOrNickname), "E-mail oraz nazwa użytkownika nie pasują do żadnego użytkownika");
                    }
                });
        }
    }
}
