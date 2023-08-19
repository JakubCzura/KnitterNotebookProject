using KnitterNotebook.Models.Entities;
using System;

namespace KnitterNotebook.Models.Dtos
{
    public class ProjectImageDto
    {
        public ProjectImageDto(ProjectImage projectImage)
        {
            Id = projectImage.Id;
            Path = projectImage.Path;
            DateTime = projectImage.DateTime;
        }

        public int Id { get; set; }

        public string Path { get; set; }

        public DateTime DateTime { get; set; }
    }
}