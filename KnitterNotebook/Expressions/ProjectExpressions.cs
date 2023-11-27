using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System;
using System.Linq.Expressions;

namespace KnitterNotebook.Expressions;

/// <summary>
/// Class to create LINQ queries for <see cref="Project"/> entity
/// </summary>
public static class ProjectExpressions
{
    public static readonly Expression<Func<Project, object>>[] IncludeNeedlesYarnsPattern = new Expression<Func<Project, object>>[]
    {
        x => x.Needles,
        x => x.Yarns,
        x => x.PatternPdf!,
    };

    public static readonly Expression<Func<Project, object>>[] IncludeNeedlesYarnsPatternImages = new Expression<Func<Project, object>>[]
    {
        x => x.Needles,
        x => x.Yarns,
        x => x.PatternPdf!,
        x => x.ProjectImages,
    };

    public static Expression<Func<Project, bool>> GetUserProjectByStatus(int userId, ProjectStatusName projectStatusName)
       => p => p.UserId == userId && p.ProjectStatus == projectStatusName;
}