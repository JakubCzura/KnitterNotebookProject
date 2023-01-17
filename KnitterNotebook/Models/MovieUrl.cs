using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models
{
    public class MovieUrl
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public Uri Link { get; set; } = null!;

        public User User { get; set; } = new();

        public int UserId { get; set; }
    }
}
