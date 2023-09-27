using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ChangeProjectStatusDtoValidator : AbstractValidator<ChangeProjectStatusDto>
{
    private readonly IProjectService _projectService;

    public ChangeProjectStatusDtoValidator(IProjectService projectService)
    {
        _projectService = projectService;

        RuleFor(x => x.ProjectId)
            .MustAsync(async (id, cancellationToken) => await _projectService.ProjectExistsAsync(id))
            .WithMessage(Translations.ProjectNotFound);

        RuleFor(x => x.ProjectStatus)
            .IsInEnum()
            .WithMessage(Translations.InvalidProjectStatus);
    }
}