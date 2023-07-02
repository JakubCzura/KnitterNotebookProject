using System;

namespace KnitterNotebook.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Today;

        public string Path { get; set; } = string.Empty;

        public Project Project { get; set; } = new();
    }
}