using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators
{
    public class EditPlannedProjectDtoValidator : AbstractValidator<EditPlannedProjectDto>
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public EditPlannedProjectDtoValidator(IUserService userService, IProjectService projectService)
        {
            _userService = userService;
            _projectService = projectService;

            RuleFor(dto => dto.Id)
                .MustAsync(async (id, cancellationToken) => await _projectService.ProjectExistsAsync(id))
                .WithMessage(Translations.ProjectNotFound);

            RuleFor(dto => dto)
                .SetValidator(new PlanProjectDtoValidator(_userService));
        }
    }
}