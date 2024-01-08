using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Entities;

public class Project : BaseDbEntity
{
    public Project()
    {
    }

    public Project(PlanProjectDto planProjectDto, List<Needle> needles, List<Yarn> yarns, ProjectStatusName projectStatusName, PatternPdf? patternPdf)
    {
        Name = planProjectDto.Name;
        StartDate = planProjectDto.StartDate;
        Needles = needles;
        Yarns = yarns;
        Description = planProjectDto.Description;
        ProjectStatus = projectStatusName;
        PatternPdf = patternPdf;
        UserId = planProjectDto.UserId;
    }

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