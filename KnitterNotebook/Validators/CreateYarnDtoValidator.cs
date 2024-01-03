using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;

namespace KnitterNotebook.Validators;

public class CreateYarnDtoValidator : AbstractValidator<CreateYarnDto>
{
    public CreateYarnDtoValidator() => RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage(Translations.YarnNameCantBeEmpty)
        .MaximumLength(100)
        .WithMessage(Translations.YarnNameMax100Chars);
}