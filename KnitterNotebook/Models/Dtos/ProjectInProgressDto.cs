using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models.Dtos
{
    public class ProjectInProgressDto : BasicProjectDto
    {
        public ProjectInProgressDto(Project project) : base(project)
        {
            Needles = project.Needles.Select(x => new NeedleDto(x)).ToList();
            Yarns = project.Yarns.Select(x => new YarnDto(x)).ToList();
            ProjectImages = project.ProjectImages.Select(x => new ProjectImageDto(x)).ToList();
        }

        public List<NeedleDto> Needles { get; set; }

        public List<YarnDto> Yarns { get; set; }

        public List<ProjectImageDto> ProjectImages { get; set; }
    }
}