using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Themes
{
    public class ApplicationThemes
    {
        public enum Themes
        {
            Default = 0,
            Light = 1,
            Dark = 2
        }

        public static List<string> GetThemes()
        {
            return Enum.GetValues(typeof(Themes))
                       .Cast<Themes>()
                       .Select(v => v.ToString())
                       .ToList();
        }
    }
}
