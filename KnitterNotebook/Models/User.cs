using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public virtual List<Project> Projects { get; set; } = new();

        public virtual List<Sample> Samples { get; set; } = new();

        public virtual List<MovieUrl> MovieUrls { get; set; } = new();

        public Theme Theme { get; set; }
    }
}