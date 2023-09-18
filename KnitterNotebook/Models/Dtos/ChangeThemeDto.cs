using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos;

public record ChangeThemeDto(int UserId, ApplicationTheme ThemeName);