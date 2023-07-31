using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public static class ProjectsFilter
    {
        public static ObservableCollection<Project> FilterByName(this IEnumerable<Project> projects, string name)
           => new(projects.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
    }
}