using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public List<Project> Projects { get; set; } = new();

        public List<Sample> Samples { get; set; } = new();

        public List<MovieUrl> MovieUrls { get; set; } = new();

        public Theme? Theme { get; set; } = null;
    }
}