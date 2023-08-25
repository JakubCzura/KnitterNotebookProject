using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class PlanProjectDtoValidator : AbstractValidator<PlanProjectDto>
    {
        private readonly IUserService _userService;

        public PlanProjectDtoValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Nazwa projektu nie może być pusta")
                .Length(1, 100).WithMessage("Długość nazwy projektu musi mieć 1-100 znaków");

            //StartDate can be null, but if it's not null it must be greater or equal today
            RuleFor(dto => dto.StartDate)
                .Must(x => !x.HasValue || x.Value.Date.CompareTo(DateTime.Today) >= 0)
                .WithMessage("Data rozpoczęcia projektu nie może być przed dzisiejszym dniem");

            RuleFor(dto => dto.Needles)
                .Must(x => x is not null && x.Any()).WithMessage("Zbiór drutów nie może być pusty")
                .ForEach(needle =>
                {
                    needle.SetValidator(new CreateNeedleDtoValidator());
                });

            RuleFor(x => x.Yarns)
                .Must(x => x is not null && x.Any()).WithMessage("Zbiór nazw włóczek nie może być pusty")
                .ForEach(yarn =>
                {
                    yarn.SetValidator(new CreateYarnDtoValidator());
                });

            RuleFor(dto => dto.Description)
                .MaximumLength(300).WithMessage("Długość opisu nie może być większa niż 300 znaków");

            RuleFor(x => x.SourcePatternPdfPath)
                .Must(x => x is null || FileExtensionValidator.IsPdf(x))
                .WithMessage("Wybierz plik z poprawnym rozszerzeniem .pdf lub usuń odnośnik do wzoru");

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
                .WithMessage("Nie znaleziono użytkownika");
        }
    }
}