using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models.Dtos
{
    public class NeedleToPlanProjectDto
    {
        public double? Size { get; set; } = null;

        public string SizeUnit { get; set; } = NeedleSizeUnits.Units.mm.ToString();
    }
}
