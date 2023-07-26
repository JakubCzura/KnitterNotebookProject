using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public string PatternName { get; set; } = string.Empty;

        public virtual List<Needle> Needles { get; set; } = new();

        public virtual List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
        public string? Description { get; set; } = null;

        public ProjectStatus ProjectStatus { get; set; }

        public int ProjectStatusId { get; set; }

        public virtual PatternPdf? PatternPdf { get; set; } = null;

        public virtual List<ProjectImage> ProjectImages { get; set; } = new();

        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}