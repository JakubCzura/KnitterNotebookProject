using KnitterNotebook.Models.Dtos;
using System;

namespace KnitterNotebook.Helpers.Filters;

public static class ProjectsFilter
{
    public enum NamesComparison
    {
        Contains,
        Equals
    }

    /// <summary>
    /// Filters <paramref name="projectToFilter"/> by <paramref name="projectName"/> and specified <paramref name="namesComparison"/>
    /// </summary>
    /// <typeparam name="T">Type of project</typeparam>
    /// <param name="projectToFilter">Project to filter</param>
    /// <param name="projectName">Name of project</param>
    /// <param name="namesComparison">Rule how to filter</param>
    /// <returns>True if filtering is successful, otherwise false</returns>
    public static bool FilterByName<T>(object projectToFilter,
                                       string projectName,
                                       NamesComparison namesComparison = NamesComparison.Contains) where T : BasicProjectDto
        => namesComparison switch
            {
                NamesComparison.Contains => projectToFilter is T project && project.Name.Contains(projectName, StringComparison.OrdinalIgnoreCase),
                NamesComparison.Equals => projectToFilter is T project && project.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase),
                _ => false
            };
}