using KnitterNotebook.Models.Dtos;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KnitterNotebook.Converters.XAMLConverters;

/// <summary>
/// Converts <see cref="BasicSampleDto"/>'s RowsQuantity and LoopsQuantity to a string. Works in Mode=OneWay only.
/// </summary>
public class SampleDtoLoopsRowsToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is BasicSampleDto sample
            ? $"{sample.LoopsQuantity}x{sample.RowsQuantity}"
            : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}