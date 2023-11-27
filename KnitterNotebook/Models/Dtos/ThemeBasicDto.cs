using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos;

public class ThemeBasicDto(Theme theme)
{
    public int Id { get; set; } = theme.Id;

    public ApplicationTheme ApplicationTheme { get; set; } = theme.Name;
}