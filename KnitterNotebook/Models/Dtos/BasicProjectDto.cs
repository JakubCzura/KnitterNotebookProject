using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System;

namespace KnitterNotebook.Models.Dtos
{
    public class BasicProjectDto
    {
        public BasicProjectDto(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            StartDate = project.StartDate;
            Description = project.Description;
            ProjectStatus = project.ProjectStatus;
            PatternPdfPath = project.PatternPdf?.Path;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public string? Description { get; set; }

        public ProjectStatusName ProjectStatus { get; set; }

        public string? PatternPdfPath { get; set; }
    }
}