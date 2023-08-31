using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Entities
{
    public class User : BaseDbEntity
    {
        public User()
        { }

        public User(int id, string nickname, string email, string password, List<Project> projects, List<Sample> samples, List<MovieUrl> movieUrls, Theme theme)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
            Password = password;
            Projects = projects;
            Samples = samples;
            MovieUrls = movieUrls;
            Theme = theme;
        }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public virtual List<Project> Projects { get; set; } = new();

        public virtual List<Sample> Samples { get; set; } = new();

        public virtual List<MovieUrl> MovieUrls { get; set; } = new();

        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiresDate { get; set; }

        public Theme Theme { get; set; }

        public int ThemeId { get; set; }
    }
}