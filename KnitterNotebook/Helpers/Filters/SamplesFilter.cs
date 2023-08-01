using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers.Filters
{
    public static class SamplesFilter
    {
        public static IEnumerable<Sample> FilterByNeedleSize(IEnumerable<Sample> samples, double needleSize, string needleSizeUnit)
            => samples.Where(x => Math.Abs(x.NeedleSize - needleSize) <= 0.0001 && x.NeedleSizeUnit.Equals(needleSizeUnit, StringComparison.OrdinalIgnoreCase));
    }
}