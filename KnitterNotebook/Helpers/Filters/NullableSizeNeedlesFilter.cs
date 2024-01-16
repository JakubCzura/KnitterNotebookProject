using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers.Filters;

public class NullableSizeNeedlesFilter
{
    /// <summary>
    /// Finds needles with size greater than 0 in specified collection
    /// </summary>
    /// <param name="needles">Needles to filter</param>
    /// <returns>Collection of needles with size greater than 0</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="needles"/> are null</exception>
    public static IEnumerable<NullableSizeNeedle> GetNeedlesWithPositiveSizeValue(params NullableSizeNeedle[] needles)
        => needles.Where(needle => needle.Size.HasValue && needle.Size > 0);
}