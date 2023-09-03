using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Entities
{
    public class User : BaseDbEntity
    {
        public User()
        { }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public virtual List<Project> Projects { get; set; } = new();

        public virtual List<Sample> Samples { get; set; } = new();

        public virtual List<MovieUrl> MovieUrls { get; set; } = new();

        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpirationDate { get; set; }

        public Theme Theme { get; set; }

        public int ThemeId { get; set; }
    }
}