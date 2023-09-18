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

    public static bool FilterByName<T>(object plannedProjectToFilter, string projectName, NamesComparison namesComparison = NamesComparison.Contains) where T : BasicProjectDto
         => namesComparison switch
         {
             NamesComparison.Contains => plannedProjectToFilter is T project && project.Name.Contains(projectName, StringComparison.OrdinalIgnoreCase),
             NamesComparison.Equals => plannedProjectToFilter is T project && project.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase),
             _ => false
         };
}