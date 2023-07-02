using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class ProjectStatus
    {
        public int Id { get; set; }

        public string Status { get; set; } = string.Empty;

        public virtual List<Project> Projects { get; set; } = new();
    }
}