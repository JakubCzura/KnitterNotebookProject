using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public class BasicProjectDto
    {
        public BasicProjectDto(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            ProjectStatus = project.ProjectStatus;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ProjectStatusName ProjectStatus { get; set; }
    }
}