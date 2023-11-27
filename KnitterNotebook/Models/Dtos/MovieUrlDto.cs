using KnitterNotebook.Models.Entities;
using System;

namespace KnitterNotebook.Models.Dtos;

public class MovieUrlDto(MovieUrl movieUrl)
{
    public int Id { get; set; } = movieUrl.Id;

    public string Title { get; set; } = movieUrl.Title;

    public string? Description { get; set; } = movieUrl.Description;

    public Uri Link { get; set; } = movieUrl.Link;
}