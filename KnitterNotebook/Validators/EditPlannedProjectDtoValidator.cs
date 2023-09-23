using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators
{
    public class EditPlannedProjectDtoValidator : AbstractValidator<EditPlannedProjectDto>
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        public EditPlannedProjectDtoValidator(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;

            RuleFor(dto => dto.Id)
               .MustAsync(async (id, cancellationToken) => await _projectService.ProjectExistsAsync(id))
               .WithMessage("Nie znaleziono projektu");

            RuleFor(dto => dto)
                .SetValidator(new PlanProjectDtoValidator(_userService));
        }
    }
}