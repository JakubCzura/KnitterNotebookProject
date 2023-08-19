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
            ProjectImages = project.ProjectImages.Select(x => new ProjectImageDto(x)).ToList();
        }

        public DateTime? EndDate { get; set; }

        public List<ProjectImageDto> ProjectImages { get; set; }
    }
}