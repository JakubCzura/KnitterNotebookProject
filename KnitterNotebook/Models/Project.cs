using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; } = null!;

        public DateTime? EndDate { get; set; } = null!;

       // public string YarnName { get; set; } = string.Empty;

       // public string PatternName { get; set; } = string.Empty;
        
        //public virtual List<Needle> Needles { get; set; } = new();
        
        //public virtual List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
       // public string Description { get; set; } = string.Empty;

       // public Status Status { get; set; } = new(); 

        //public string PDFFilePath { get; set; } = string.Empty;

        //public virtual List<Image> ProjectImages = new();

        public virtual User User { get; set; } = new();
    }
}