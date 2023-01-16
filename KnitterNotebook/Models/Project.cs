using System;

namespace KnitterNotebook.Models
{
    public class Project
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; } = null!;

        public DateTime? EndDate { get; set; } = null!;

        public int UserId { get; set; }

        public User User { get; set; } = new();
    }
}