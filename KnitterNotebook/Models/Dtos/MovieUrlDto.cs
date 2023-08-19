using KnitterNotebook.Models.Entities;
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
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public Uri Link { get; set; }
    }
}