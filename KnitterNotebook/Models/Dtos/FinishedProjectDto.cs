using KnitterNotebook.Models.Entities;
using System;

namespace KnitterNotebook.Models.Dtos;

public class FinishedProjectDto : ProjectInProgressDto
{
    public FinishedProjectDto(Project project) : base(project)
    {
        EndDate = project.EndDate;
    }

    public DateTime? EndDate { get; set; }
}