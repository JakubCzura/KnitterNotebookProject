using System;

namespace KnitterNotebook.Models.Entities;

public class MovieUrl : BaseDbEntity
{
    public string Title { get; set; } = string.Empty;

    public virtual Uri Link { get; set; } = default!;

    public string? Description { get; set; }

    public int UserId { get; set; }
}