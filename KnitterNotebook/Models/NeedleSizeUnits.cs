using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models
{
    public class NeedleSizeUnits
    {
        public enum Units
        {
            mm,
            cm
        }

        public static IEnumerable<string> UnitsList => Enum.GetNames(typeof(Units));
    }
}
