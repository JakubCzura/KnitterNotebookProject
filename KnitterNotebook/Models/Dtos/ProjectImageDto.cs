using KnitterNotebook.Models.Entities;
using System;

namespace KnitterNotebook.Models.Dtos;

public class ProjectImageDto(ProjectImage projectImage)
{
    public int Id { get; set; } = projectImage.Id;

    public string Path { get; set; } = projectImage.Path;

    public DateTime DateTime { get; set; } = projectImage.DateTime.ToLocalTime();
}