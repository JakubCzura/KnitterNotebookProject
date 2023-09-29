using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Converters
{
    public class YarnsNamesWithDelimiterConverter
    {
        /// <summary>
        /// Converts <paramref name="yarns"/>' names to string with <paramref name="delimiter"/> as separator.
        /// </summary>
        /// <param name="yarns">Yarns to convert</param>
        /// <param name="delimiter">Delimiter to join yarns' names as string</param>
        /// <returns><paramref name="yarns"/>' names as string</returns>
        /// <exception cref="ArgumentNullException"> when <paramref name="yarns"/> are null</exception>"
        public static string Convert(IEnumerable<YarnDto> yarns, char delimiter = ',') => string.Join(delimiter, yarns.Select(x => x.Name));
    }
}