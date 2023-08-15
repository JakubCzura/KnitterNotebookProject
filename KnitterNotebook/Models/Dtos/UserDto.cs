using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
        }

        public UserDto(User user)
        {
            Id = user.Id;
            Nickname = user.Nickname;
            Email = user.Email;
            ThemeName = user.Theme.Name;
        }

        public int Id { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public ApplicationTheme ThemeName { get; set; }
    }
}