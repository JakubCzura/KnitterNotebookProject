using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using System;

namespace KnitterNotebook.Helpers.Filters;

public static class SamplesFilter
{
    /// <summary>
    /// Filters <paramref name="sampleToFilter"/> by <paramref name="needleSize"/> and specified <paramref name="needleSizeUnit"/>
    /// </summary>
    /// <typeparam name="T">Type of sample to filter</typeparam>
    /// <param name="sampleToFilter">Sample to filter</param>
    /// <param name="needleSize">Size of sample's needle. It can be null as the function is expected to filter collection in view model so when user doesn't type needle's size the validation returns true</param>
    /// <param name="needleSizeUnit">Unit of measurement</param>
    /// <returns>True if filtering is successful, otherwise false</returns>
    public static bool FilterByNeedleSize<T>(object sampleToFilter,
                                             double? needleSize,
                                             NeedleSizeUnit needleSizeUnit) where T : BasicSampleDto
         => !needleSize.HasValue 
            || sampleToFilter is T sample && Math.Abs(sample.NeedleSize - needleSize.Value) <= 0.0001 && sample.NeedleSizeUnit == needleSizeUnit;
}