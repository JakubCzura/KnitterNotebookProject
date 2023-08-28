using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators
{
    public class CreateProjectImageDtoValidator : AbstractValidator<CreateProjectImageDto>
    {
        private readonly IProjectService _projectService;

        public CreateProjectImageDtoValidator(IProjectService projectService)
        {
            _projectService = projectService;

            RuleFor(x => x.ImagePath)
               .Must(FileExtensionValidator.IsImage).WithMessage("Wybierz zdjęcie z innym formatem: .jpg, .jpeg, .png lub usuń odnośnik do zdjęcia");

            RuleFor(x => x.ProjectId)
               .MustAsync(async (id, cancellationToken) => await _projectService.ProjectExistsAsync(id)).WithMessage("Nie odnaleziono projektu");
        }
    }
}