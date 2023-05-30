using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Today;

        public string Path { get; set; } = string.Empty;
    }
}
