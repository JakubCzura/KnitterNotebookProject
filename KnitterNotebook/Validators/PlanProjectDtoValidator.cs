using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class PlanProjectDtoValidator : AbstractValidator<PlanProjectDto>
    {
        private readonly DatabaseContext _databaseContext;

        public PlanProjectDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Nazwa projektu nie może być pusta")
                .Length(1, 100).WithMessage("Długość nazwy projektu musi mieć 1-100 znaków");

            RuleFor(dto => dto.StartDate)
                .Must(x => x is null || x.HasValue && x.Value.Date.CompareTo(DateTime.Today) >= 0)
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
                .Must(x => x is null || PdfExtensionValidator.IsPdf(x))
                .WithMessage("Wybierz plik z poprawnym rozszerzeniem .pdf lub usuń odnośnik do wzoru");

            RuleFor(dto => dto.UserId)
                .MustAsync(async (id, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == id, cancellationToken))
                .WithMessage("Nie znaleziono użytkownika");
        }
    }
}