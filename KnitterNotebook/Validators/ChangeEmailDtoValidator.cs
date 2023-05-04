using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Validators
{
    public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
    {
        private readonly IUserService _userService;
        
        public ChangeEmailDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(dto => dto.UserId)
                .Must(id => _userService.GetAsync(id) != null)
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail nie może być pusty")
                .MaximumLength(50).WithMessage("E-mail może mieć maksimum 50 znaków")
                .EmailAddress().WithMessage("Niepoprawny format e-mail");
            
            RuleFor(x => x.Email)
                .CustomAsync(async (value, context, cancellationToken) =>
                {
                    IEnumerable<User> users = await _userService.GetAllAsync();
                    bool isEmailUsed = users.Any(x => x.Email == value);
                    if (isEmailUsed)
                    {
                        context.AddFailure(nameof(RegisterUserDto.Email), "E-mail jest już używany");
                    }
                });
        }
    }
}
