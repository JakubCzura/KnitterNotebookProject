using System;
using System.Collections.Generic;

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