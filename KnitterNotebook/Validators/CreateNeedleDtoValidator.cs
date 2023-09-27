using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;

namespace KnitterNotebook.Validators;

public class CreateNeedleDtoValidator : AbstractValidator<CreateNeedleDto>
{
    public CreateNeedleDtoValidator()
    {
        RuleFor(x => x.Size)
            .InclusiveBetween(0.1, 100)
            .WithMessage($"{Translations.NeedleSizeMustBeBetween} 0.1 - 100");

        RuleFor(x => x.SizeUnit)
            .IsInEnum()
            .WithMessage(Translations.InvalidSizeUnit);
    }
}