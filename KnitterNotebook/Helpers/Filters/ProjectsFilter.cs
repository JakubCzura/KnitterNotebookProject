using KnitterNotebook.Models;
using KnitterNotebook.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KnitterNotebook.Helpers.Filters
{
    public static class ProjectsFilter
    {
        public enum NamesComparison
        {
            Contains,
            Equals
        }

        public static IEnumerable<Project> FilterByName(IEnumerable<Project> projects, string projectName, NamesComparison namesComparison = NamesComparison.Contains)
           => namesComparison switch
             {
                 NamesComparison.Contains => projects.Where(x => x.Name.Contains(projectName, StringComparison.OrdinalIgnoreCase)),
                 NamesComparison.Equals => projects.Where(x => x.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase)),
                 _ => projects.Where(x => x.Name.Contains(projectName, StringComparison.OrdinalIgnoreCase))
             };
    }
}