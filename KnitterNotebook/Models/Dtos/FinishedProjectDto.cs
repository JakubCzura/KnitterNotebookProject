using KnitterNotebook.Models.Entities;
using System;

namespace KnitterNotebook.Models.Dtos;

public class FinishedProjectDto(Project project) : ProjectInProgressDto(project)
{
    public DateTime? EndDate { get; set; } = project.EndDate;
}