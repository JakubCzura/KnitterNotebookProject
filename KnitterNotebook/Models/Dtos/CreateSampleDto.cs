using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models.Dtos
{
    public class CreateSampleDto
    {
        public string YarnName { get; set; } = string.Empty;

        public int LoopsQuantity { get; set; }

        public int RowsQuantity { get; set; }

        public int NeedleSize { get; set; }

        public string NeedleSizeUnit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
