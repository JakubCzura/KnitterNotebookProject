using FluentValidation;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class ChangeNicknameDtoValidator : AbstractValidator<ChangeNicknameDto>
    {
        private readonly IUserService _userService;

        public ChangeNicknameDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.Nickname)
              .NotEmpty().WithMessage("Nazwa użytkownika nie może być pusta")
              .MinimumLength(1).WithMessage("Nazwa użytkownika musi mieć conajmniej 1 znak")
              .MaximumLength(50).WithMessage("Nazwa użytkownika może mieć maksimum 50 znaków");

            RuleFor(x => x.Nickname)
               .CustomAsync(async (value, context, cancellationToken) =>
               {
                   IEnumerable<User> users = await _userService.GetAllAsync();
                   bool isNicknameUsed = users.Any(x => x.Nickname == value);
                   if (isNicknameUsed)
                   {
                       context.AddFailure(nameof(RegisterUserDto.Nickname), "Nick jest już używany");
                   }
               });
        }
    }
}