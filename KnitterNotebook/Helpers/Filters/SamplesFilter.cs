using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using System;

namespace KnitterNotebook.Helpers.Filters;

public static class SamplesFilter
{
    public static bool FilterByNeedleSize<T>(object sampleToFilter, double? needleSize, NeedleSizeUnit needleSizeUnit) where T : BasicSampleDto
         => !needleSize.HasValue ||
             sampleToFilter is T sample && Math.Abs(sample.NeedleSize - needleSize.Value) <= 0.0001 && sample.NeedleSizeUnit == needleSizeUnit;
}