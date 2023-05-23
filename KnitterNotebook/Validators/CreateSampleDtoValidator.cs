using FluentValidation;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Validators
{
    public class CreateSampleDtoValidator : AbstractValidator<CreateSampleDto>
    {
        public CreateSampleDtoValidator()
        {
            RuleFor(x => x.YarnName)
                .NotEmpty().WithMessage("Nazwa włóczki nie może być pusta")
                .MaximumLength(100).WithMessage("Nazwa włóczki może mieć maksimum 100 znaków");

            RuleFor(x => x.LoopsQuantity)
                .LessThanOrEqualTo(int.MaxValue).WithMessage("Zbyt duża ilość oczek");

            RuleFor(x => x.RowsQuantity)
                .LessThanOrEqualTo(int.MaxValue).WithMessage("Zbyt duża ilość rzędów");

            RuleFor(x => x.NeedleSize)
                .LessThanOrEqualTo(double.MaxValue).WithMessage("Zbyt duży rozmiar drutu");

            RuleFor(x => x.NeedleSizeUnit)
                .NotEmpty().WithMessage("Jednostka miary nie może być pusta")
                .MaximumLength(100).WithMessage("Jednostka miary może mieć maksimum 100 znaków");

            RuleFor(x => x.Description)
                .MaximumLength(10000).WithMessage("Opis może mieć maksymalnie 10000 znaków");

            RuleFor(x => x.ImagePath)
                .MaximumLength(1000).WithMessage("Długość ścieżki do zapisu zdjęcia nie może być większa niż 1000 znaków");
        }
    }
}