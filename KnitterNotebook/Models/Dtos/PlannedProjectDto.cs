using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models.Dtos;

public class PlannedProjectDto : BasicProjectDto
{
    public PlannedProjectDto(Project project) : base(project)
    {
        Needles = project.Needles.Select(x => new NeedleDto(x)).ToList();
        Yarns = project.Yarns.Select(x => new YarnDto(x)).ToList();
    }

    public List<NeedleDto> Needles { get; set; }

    public List<YarnDto> Yarns { get; set; }
}