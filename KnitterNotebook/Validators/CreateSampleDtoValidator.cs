using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators
{
    public class CreateSampleDtoValidator : AbstractValidator<CreateSampleDto>
    {
        private readonly IUserService _userService;

        public CreateSampleDtoValidator(IUserService userService)
        {
            _userService = userService;

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
               .IsInEnum().WithMessage("Proszę podać prawidłową jednostkę długości");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis może mieć maksymalnie 1000 znaków");

            RuleFor(x => x.SourceImagePath)
                .Must(x => x is null || FileExtensionValidator.IsImage(x))
                .WithMessage("Wybierz zdjęcie z innym formatem: .jpg, .jpeg, .png lub usuń odnośnik do zdjęcia");

            RuleFor(x => x.UserId)
                .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
                .WithMessage("Nie odnaleziono użytkownika");
        }
    }
}