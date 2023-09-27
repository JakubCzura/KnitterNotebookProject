using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class CreateProjectImageDtoValidator : AbstractValidator<CreateProjectImageDto>
{
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    public CreateProjectImageDtoValidator(IProjectService projectService, IUserService userService)
    {
        _projectService = projectService;
        _userService = userService;

        RuleFor(x => x.ImagePath)
           .Must(FileExtensionValidator.IsImage)
           .WithMessage(Translations.PhotoValidExtension);

        RuleFor(x => x.ProjectId)
           .MustAsync(async (id, cancellationToken) => await _projectService.ProjectExistsAsync(id))
           .WithMessage(Translations.ProjectNotFound);

        RuleFor(x => x.UserId)
           .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
           .WithMessage(Translations.UserNotFound);
    }
}