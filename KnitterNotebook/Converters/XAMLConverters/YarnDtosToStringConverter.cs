using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace KnitterNotebook.Converters.XAMLConverters;

/// <summary>
/// Converts collection of <see cref="YarnDto"/> to a string. Works in Mode=OneWay only.
/// </summary>
public class YarnDtosToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is IEnumerable<YarnDto> yarns ? string.Join("\n", yarns.Select(x => x.Name)) : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}