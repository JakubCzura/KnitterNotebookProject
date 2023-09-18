using KnitterNotebook.Models.Entities;
using System;

namespace KnitterNotebook.Models.Dtos;

public class MovieUrlDto
{
    public MovieUrlDto(MovieUrl movieUrl)
    {
        Id = movieUrl.Id;
        Title = movieUrl.Title;
        Description = movieUrl.Description;
        Link = movieUrl.Link;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public Uri Link { get; set; }
}