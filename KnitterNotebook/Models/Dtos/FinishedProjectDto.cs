using KnitterNotebook.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models.Dtos
{
    public class FinishedProjectDto : BasicProjectDto
    {
        public FinishedProjectDto(Project project) : base(project)
        {
            EndDate = project.EndDate;
            Needles = project.Needles.Select(x => new NeedleDto(x)).ToList();
            Yarns = project.Yarns.Select(x => new YarnDto(x)).ToList();
            ProjectImages = project.ProjectImages.Select(x => new ProjectImageDto(x)).ToList();
        }

        public DateTime? EndDate { get; set; }

        public List<NeedleDto> Needles { get; set; }

        public List<YarnDto> Yarns { get; set; }

        public List<ProjectImageDto> ProjectImages { get; set; }
    }
}