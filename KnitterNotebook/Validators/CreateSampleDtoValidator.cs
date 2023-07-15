using FluentValidation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.IO;

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

            RuleFor(x => x.NeedleSizeUnit)
                .Must(value => value == "mm" || value == "cm")
                .WithMessage("Jednostka miary może być określona tylko jako 'cm' lub 'mm' ");

            RuleFor(x => x.Description)
                .MaximumLength(10000).WithMessage("Opis może mieć maksymalnie 10000 znaków");

            RuleFor(x => x.SourceImagePath)
                .Must(x => x is null || ImageExtensionValidator.IsImage(x))
                .WithMessage("Wybierz zdjęcie z innym formatem: .jpg, .jpeg, .png, .gif, .bmp lub usuń odnośnik do zdjęcia");

            RuleFor(x => x.DestinationImagePath)
                .Must(x => x is null || ImageExtensionValidator.IsImage(x))
                .WithMessage("Wybierz zdjęcie z innym formatem: .jpg, .jpeg, .png, .gif, .bmp lub usuń odnośnik do zdjęcia")
                .Must(x => !File.Exists(x))
                .WithMessage("Plik o podanej nazwie już istnieje, podaj inny plik lub zmień jego nazwę przed wyborem");      

            RuleFor(x => x.UserId)
             .MustAsync(async (value, cancellationToken) => await _databaseContext.Users.AnyAsync(x => x.Id == value, cancellationToken))
             .WithMessage("Nie odnaleziono użytkownika");
        }
    }
}