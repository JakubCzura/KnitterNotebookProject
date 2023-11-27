using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models.Dtos;

public class ProjectInProgressDto(Project project) : PlannedProjectDto(project)
{
    public List<ProjectImageDto> ProjectImages { get; set; } = project.ProjectImages.Select(x => new ProjectImageDto(x)).ToList();
}