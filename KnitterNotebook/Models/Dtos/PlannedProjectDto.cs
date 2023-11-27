using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models.Dtos;

public class PlannedProjectDto(Project project) : BasicProjectDto(project)
{
    public List<NeedleDto> Needles { get; set; } = project.Needles.Select(x => new NeedleDto(x)).ToList();

    public List<YarnDto> Yarns { get; set; } = project.Yarns.Select(x => new YarnDto(x)).ToList();
}