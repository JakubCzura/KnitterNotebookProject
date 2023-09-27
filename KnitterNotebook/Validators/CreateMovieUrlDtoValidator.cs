using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class CreateMovieUrlDtoValidator : AbstractValidator<CreateMovieUrlDto>
{
    private readonly IUserService _userService;

    public CreateMovieUrlDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(Translations.TitleCantBeEmpty)
            .MaximumLength(100)
            .WithMessage(Translations.TitleMax100Chars);

        RuleFor(x => x.Link)
            .NotEmpty()
            .WithMessage(Translations.LinkCantBeEmpty);

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .WithMessage($"{Translations.DescriptionMaxChars} 100 {Translations.characters}");

        RuleFor(dto => dto.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage(Translations.UserNotFound);
    }
}