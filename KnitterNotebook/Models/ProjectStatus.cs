using KnitterNotebook.Models.Enums;
using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class ProjectStatus
    {
        public ProjectStatus()
        {
        }

        public ProjectStatus(int id, ProjectStatusName status)
        {
            Id = id;
            Status = status;
        }

        public int Id { get; set; }

        public ProjectStatusName Status { get; set; } = ProjectStatusName.Planned;

        public virtual List<Project> Projects { get; set; } = new();
    }
}