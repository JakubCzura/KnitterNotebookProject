using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

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

            RuleFor(x => x.Description)
              .MaximumLength(100).WithMessage("Opis filmu nie może mieć więcej niż 100 znaków");

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
                .WithMessage("Nie znaleziono użytkownika");
        }
    }
}