using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
{
    private readonly IUserService _userService;

    public ChangeEmailDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(dto => dto.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage("Nie znaleziono użytkownika");

        RuleFor(x => x.Email)
            .NotNull().WithMessage("Wartość nie może być pusta")
            .MustAsync(async (email, cancellationToken) => !await _userService.IsEmailTakenAsync(email))
            .WithMessage("E-mail jest już używany")
            .EmailAddress().WithMessage("Niepoprawny format e-mail");
    }
}