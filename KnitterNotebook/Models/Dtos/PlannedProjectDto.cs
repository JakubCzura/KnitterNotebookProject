using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Collections.Generic;
using System;

namespace KnitterNotebook.Models.Dtos
{
    public class PlannedProjectDto
    {
        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; } = null;

        public virtual List<Needle> Needles { get; set; } = new();

        public virtual List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
        public string? Description { get; set; } = null;

        public ProjectStatusName ProjectStatus { get; set; }

        public string PatternPdfPath { get; set; } = null;
    }
}