using FluentValidation;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Validators
{
    public class CreateNeedleDtoValidator : AbstractValidator<CreateNeedleDto>
    {
        public CreateNeedleDtoValidator()
        {
            RuleFor(x => x.Size)
                .InclusiveBetween(0.1, 100).WithMessage("Rozmiar drutu musi być z zakresu 0.1-100");

            RuleFor(x => x.SizeUnit)
                .IsInEnum().WithMessage("Proszę podać prawidłową jednostkę długości");
        }
    }
}