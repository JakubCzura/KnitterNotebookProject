using System;

namespace KnitterNotebook.Models.Dtos
{
    public class MovieUrlDto
    {
        public MovieUrlDto(MovieUrl movieUrl)
        {
            Id = movieUrl.Id;
            Title = movieUrl.Title;
            Link = movieUrl.Link;
            UserId = movieUrl.UserId;
        }

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public Uri Link { get; set; }

        public int UserId { get; set; }
    }
}