using System;

namespace KnitterNotebook.Models.Entities
{
    public class ProjectImage : BaseImage
    {
        public DateTime DateTime { get; set; } = DateTime.Today;

        public Project Project { get; set; } = new();
    }
}