using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public List<Project> Projects { get; set; } = new();

        public List<MovieUrl> MovieUrls { get; set; } = new();

        public Theme Theme { get; set; } = new();

        public int ThemeId { get; set; }
    }
}