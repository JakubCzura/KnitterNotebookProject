using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class Theme
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual List<User> Users { get; set; } = new();
    }
}