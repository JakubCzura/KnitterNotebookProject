using KnitterNotebook.Models.Enums;
using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Entities;

public class Project : BaseDbEntity
{
    public string Name { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; } = null;

    public DateTime? EndDate { get; set; } = null;

    public virtual List<Needle> Needles { get; set; } = [];

    public virtual List<Yarn> Yarns { get; set; } = [];

    public string? Description { get; set; } = null;

    public ProjectStatusName ProjectStatus { get; set; }

    public virtual PatternPdf? PatternPdf { get; set; } = default;

    public virtual List<ProjectImage> ProjectImages { get; set; } = [];

    public int UserId { get; set; }
}