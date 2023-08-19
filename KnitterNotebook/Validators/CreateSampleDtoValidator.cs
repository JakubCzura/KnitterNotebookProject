using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnitterNotebook.Validators
{
    public class CreateSampleDtoValidator : AbstractValidator<CreateSampleDto>
    {
        private readonly DatabaseContext _databaseContext;

        public CreateSampleDtoValidator(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            RuleFor(x => x.YarnName)
                .NotEmpty().WithMessage("Nazwa włóczki nie może być pusta")
                .MaximumLength(200).WithMessage("Nazwa włóczki może mieć maksimum 200 znaków");

            RuleFor(x => x.LoopsQuantity)
                .InclusiveBetween(1, 100000).WithMessage("Ilość oczek musi być z zakresu 1-100000");

            RuleFor(x => x.RowsQuantity)
                .InclusiveBetween(1, 100000).WithMessage("Ilość rzędów musi być z zakresu 1-100000");

            RuleFor(x => x.NeedleSize)
                .InclusiveBetween(0.1, 100).WithMessage("Rozmiar drutu musi być z zakresu 0.1-100");

            RuleFor(x => x.Description)
                .MaximumLength(10000).WithMessage("Opis może mieć maksymalnie 10000 znaków");

            RuleFor(x => x.SourceImagePath)
                .Must(x => x is null || FileExtensionValidator.IsImage(x))
                .WithMessage("Wybierz zdjęcie z innym formatem: .jpg, .jpeg, .png lub usuń odnośnik do zdjęcia");

            RuleFor(x => x.UserId)
             .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == value, cancellationToken))
             .WithMessage("Nie odnaleziono użytkownika");
        }
    }
}