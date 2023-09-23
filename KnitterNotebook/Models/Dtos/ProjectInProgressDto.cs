using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models.Dtos;

public class ProjectInProgressDto : PlannedProjectDto
{
    public ProjectInProgressDto(Project project) : base(project)
    {
        ProjectImages = project.ProjectImages.Select(x => new ProjectImageDto(x)).ToList();
    }

    public List<ProjectImageDto> ProjectImages { get; set; }
}