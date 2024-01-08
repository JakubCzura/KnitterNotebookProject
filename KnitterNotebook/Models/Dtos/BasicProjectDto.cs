using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System;

namespace KnitterNotebook.Models.Dtos;

public class BasicProjectDto(Project project)
{
    public int Id { get; set; } = project.Id;

    public string Name { get; set; } = project.Name;

    public DateTime? StartDate { get; set; } = project.StartDate?.ToLocalTime();

    public string? Description { get; set; } = project.Description;

    public ProjectStatusName ProjectStatus { get; set; } = project.ProjectStatus;

    public string? PatternPdfPath { get; set; } = project.PatternPdf?.Path;
}