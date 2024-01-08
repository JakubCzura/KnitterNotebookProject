using KnitterNotebook.Models.Dtos;
using System;

namespace KnitterNotebook.Models.Entities;

public class MovieUrl : BaseDbEntity
{
    public MovieUrl()
    {
    }

    public MovieUrl(CreateMovieUrlDto createMovieUrlDto)
    {
        Title = createMovieUrlDto.Title;
        Link = new Uri(createMovieUrlDto.Link);
        Description = createMovieUrlDto.Description;
        UserId = createMovieUrlDto.UserId;
    }

    public string Title { get; set; } = string.Empty;

    public virtual Uri Link { get; set; } = default!;

    public string? Description { get; set; }

    public int UserId { get; set; }
}