using System;

namespace KnitterNotebook.Models.Entities
{
    public class MovieUrl : BaseDbEntity
    {
        public string Title { get; set; } = string.Empty;

        public Uri Link { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}