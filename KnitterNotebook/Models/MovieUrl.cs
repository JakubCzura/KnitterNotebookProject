using System;

namespace KnitterNotebook.Models
{
    public class MovieUrl
    {
        public MovieUrl()
        {
        }


        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public Uri Link { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}