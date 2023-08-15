using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public class ThemeBasicDto
    {
        public ThemeBasicDto(Theme theme)
        {
            Id = theme.Id;
            ApplicationTheme = theme.Name;
        }

        public int Id { get; set; }

        public ApplicationTheme ApplicationTheme { get; set; }
    }
}