using FluentValidation;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System;
using System.Linq;

namespace KnitterNotebook.Validators
{
    public class CreateNeedleDtoValidator : AbstractValidator<CreateNeedleDto>
    {
        public CreateNeedleDtoValidator()
        {
            RuleFor(x => x.Size)
                .InclusiveBetween(0.1, 100).WithMessage("Rozmiar drutu musi być z zakresu 0.1-100");

            RuleFor(x => x.SizeUnit)
                .Must(value => NeedleSizeUnits.UnitsList.Contains(value))
                .WithMessage($"Jednostka miary może być określona tylko jako {string.Join(", ", NeedleSizeUnits.UnitsList)}");
        }
    }
}
