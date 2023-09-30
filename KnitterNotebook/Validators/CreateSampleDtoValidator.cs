using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class CreateSampleDtoValidator : AbstractValidator<CreateSampleDto>
{
    private readonly IUserService _userService;

    public CreateSampleDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.YarnName)
            .NotEmpty()
            .WithMessage(Translations.YarnNameCantBeEmpty)
            .MaximumLength(100)
            .WithMessage(Translations.YarnNameMax100Chars);

        RuleFor(x => x.LoopsQuantity)
            .InclusiveBetween(1, 100000)
            .WithMessage($"{Translations.LoopsQuantityMustBeBetween}: 1 - 100000");

        RuleFor(x => x.RowsQuantity)
            .InclusiveBetween(1, 100000)
            .WithMessage($"{Translations.RowsQuantityMustBeBetween}: 1 - 100000");

        RuleFor(x => x.NeedleSize)
            .InclusiveBetween(0.1, 100)
            .WithMessage($"{Translations.NeedleSizeMustBeBetween}: 0.1 - 100");

        RuleFor(x => x.NeedleSizeUnit)
            .IsInEnum()
            .WithMessage(Translations.InvalidSizeUnit);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage($"{Translations.DescriptionMaxChars} 1000 {Translations.characters}");

        RuleFor(x => x.SourceImagePath)
            .Must(x => x is null || FileExtensionValidator.IsImage(x))
            .WithMessage(Translations.PhotoValidExtensionOrEmpty);

        RuleFor(x => x.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage(Translations.UserNotFound);
    }
}