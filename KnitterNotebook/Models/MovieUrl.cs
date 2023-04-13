using System;

namespace KnitterNotebook.Models
{
    public class MovieUrl
    {
        public MovieUrl()
        {
        }

        public MovieUrl(string title, Uri link, User user)
        {
            Title = title;
            Link = link;
            User = user;
        }

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public Uri Link { get; set; } = null!;

        public User User { get; set; } = new();

        public int UserId { get; set; }
    }
}