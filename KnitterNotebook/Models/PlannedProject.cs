using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class PlannedProject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string YarnName { get; set; } = string.Empty;

        public string PatternName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

       // public virtual List<Needle> Needles { get; set; } = new();

       // public virtual List<Yarn> Yarns { get; set; } = new();
    }
}