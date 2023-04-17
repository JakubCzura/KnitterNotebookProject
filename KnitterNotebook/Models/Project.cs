using System;

namespace KnitterNotebook.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; } = null!;

        public DateTime? EndDate { get; set; } = null!;

        public User User { get; set; } = new();
    }
}