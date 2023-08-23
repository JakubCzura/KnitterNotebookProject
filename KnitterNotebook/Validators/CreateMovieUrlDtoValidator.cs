using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class CreateMovieUrlDtoValidator : AbstractValidator<CreateMovieUrlDto>
    {
        private readonly IUserService _userService;

        public CreateMovieUrlDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tytuł nie może być pusty");

            RuleFor(x => x.Link)
                .NotEmpty().WithMessage("Link do filmu nie może być pusty");

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _userService.UserExists(id))
                .WithMessage("Nie znaleziono użytkownika");
        }
    }
}