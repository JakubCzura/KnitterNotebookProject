using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public static class SamplesFilter
    {
        public static ObservableCollection<Sample> FilterByNeedleSize(this IEnumerable<Sample> samples, double needleSize, string needleSizeUnit)
            => new(samples.ToList().Where(x => Math.Abs(x.NeedleSize - needleSize) <= 0.0001 && x.NeedleSizeUnit.Equals(needleSizeUnit, StringComparison.OrdinalIgnoreCase)));
    }
}