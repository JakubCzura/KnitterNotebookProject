namespace KnitterNotebook.Models.Dtos
{
    public class ChangeThemeDto
    {
        public ChangeThemeDto(int userId, string themeName)
        {
            UserId = userId;
            ThemeName = themeName;
        }

        public int UserId { get; set; }

        public string ThemeName { get; set; } = string.Empty;
    }
}