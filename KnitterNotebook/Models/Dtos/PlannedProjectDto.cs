using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Collections.Generic;
using System;
using System.Linq;

namespace KnitterNotebook.Models.Dtos
{
    public class PlannedProjectDto : BasicProjectDto
    {
        public PlannedProjectDto(Project project) : base(project)
        {
            Id = project.Id;
            Name = project.Name;
            StartDate = project.StartDate;
            Needles = project.Needles.Select(x => new NeedleDto(x)).ToList();
            Yarns = project.Yarns.Select(x => new YarnDto(x)).ToList();
            Description = project.Description;
            ProjectStatus = project.ProjectStatus;
            PatternPdfPath = project.PatternPdf?.Path;
        }

        public DateTime? StartDate { get; set; }

        public List<NeedleDto> Needles { get; set; } 

        public List<YarnDto> Yarns { get; set; }

        public string? Description { get; set; }

        public string? PatternPdfPath { get; set; }
    }
}