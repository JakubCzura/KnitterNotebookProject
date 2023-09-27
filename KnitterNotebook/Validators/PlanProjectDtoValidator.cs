using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Linq;

namespace KnitterNotebook.Validators;

public class PlanProjectDtoValidator : AbstractValidator<PlanProjectDto>
{
    private readonly IUserService _userService;

    public PlanProjectDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage(Translations.ProjectNameCantBeEmpty)
            .MaximumLength(100)
            .WithMessage($"{Translations.ProjectNameMaxChars} 100 {Translations.characters}");

        //StartDate can be null, but if it's not null it must be greater or equal today
        RuleFor(dto => dto.StartDate)
            .Must(x => !x.HasValue || x.Value.Date.CompareTo(DateTime.Today) >= 0)
            .WithMessage(Translations.ProjectStartDateValidOrEmpty);

        RuleFor(dto => dto.Needles)
            .Must(x => x is not null && x.Any())
            .WithMessage(Translations.NeedleCollectionCantBeEmpty)
            .ForEach(needle =>
            {
                needle.SetValidator(new CreateNeedleDtoValidator());
            });

        RuleFor(x => x.Yarns)
            .Must(x => x is not null && x.Any())
            .WithMessage(Translations.YarnCollectionCantBeEmpty)
            .ForEach(yarn =>
            {
                yarn.SetValidator(new CreateYarnDtoValidator());
            });

        RuleFor(dto => dto.Description)
            .MaximumLength(300)
            .WithMessage($"{Translations.DescriptionMaxChars} 300 {Translations.characters}");

        RuleFor(x => x.SourcePatternPdfPath)
            .Must(x => x is null || FileExtensionValidator.IsPdf(x))
            .WithMessage(Translations.PhotoValidExtensionOrEmpty);

        RuleFor(dto => dto.UserId)
            .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
            .WithMessage(Translations.UserNotFound);
    }
}