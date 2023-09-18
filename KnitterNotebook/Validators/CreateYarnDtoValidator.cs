using FluentValidation;
using KnitterNotebook.Models.Dtos;

namespace KnitterNotebook.Validators;

public class CreateYarnDtoValidator : AbstractValidator<CreateYarnDto>
{
    public CreateYarnDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa włóczki nie może być pusta")
            .Length(1, 100).WithMessage("Nazwa włóczki musi mieć 1-100 znaków");
    }
}