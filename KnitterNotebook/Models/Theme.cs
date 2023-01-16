using System.Collections.Generic;

namespace KnitterNotebook.Models
{
    public class Theme
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public List<User> Users = new();
    }
}