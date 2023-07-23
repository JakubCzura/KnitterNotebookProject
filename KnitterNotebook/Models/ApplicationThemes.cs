using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Models
{
    public class ApplicationThemes
    {
        public enum Themes
        {
            Default = 0,
            Light = 1,
            Dark = 2
        }

        public static List<string> ThemesList() => Enum.GetValues(typeof(Themes))
                                                       .Cast<Themes>()
                                                       .Select(v => v.ToString())
                                                       .ToList();
    }
}