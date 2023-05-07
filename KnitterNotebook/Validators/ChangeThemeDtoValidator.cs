using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators
{
    public class ChangeThemeDtoValidator : AbstractValidator<ChangeThemeDto>
    {
        private readonly IUserService _userService;

        public ChangeThemeDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(dto => dto.UserId)
                .Must(id => _userService.GetAsync(id) != null)
                .WithMessage("Nie znaleziono użytkownika");

            RuleFor(dto => dto.ThemeName)
                .NotEmpty().WithName("Motyw nie może być pusty");
        }
    }
}