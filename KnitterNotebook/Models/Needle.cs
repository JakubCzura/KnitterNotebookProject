using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models
{
    public class Needle
    {
        public int Id { get; set; }

        public double Size { get; set; }

        public string SizeUnit { get; set; } = string.Empty;
    }
}
