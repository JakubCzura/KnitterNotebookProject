using KnitterNotebook.Models.Enums;
using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class Theme
    {
        public int Id { get; set; }

        public ApplicationTheme Name { get; set; } = ApplicationTheme.Default;

        public virtual List<User> Users { get; set; }
    }
}