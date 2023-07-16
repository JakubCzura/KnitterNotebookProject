using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models.Dtos
{
    class PlanProjectDto
    {
        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; } = null;

        public string YarnName { get; set; } = string.Empty;

        public string PatternName { get; set; } = string.Empty;

        //public virtual List<Needle> Needles { get; set; } = new();

        //public virtual List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
        public string Description { get; set; } = string.Empty;

        public ProjectStatus ProjectStatus { get; set; } = new();

        //public string PDFFilePath { get; set; } = string.Empty;

        //public virtual List<ProjectImage> ProjectImages = new();

        public int UserId;
    }
}
