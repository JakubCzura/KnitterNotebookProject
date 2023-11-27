using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace KnitterNotebook.Converters.XAMLConverters;

/// <summary>
/// Converts collection of <see cref="NeedleDto"/>'s Size and SizeUnit to a string. Works in Mode=OneWay only.
/// </summary>
public class NeedleDtosToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is IEnumerable<NeedleDto> needles
            ? string.Join(Environment.NewLine, needles.Select(x => $"{x.Size} {x.SizeUnit}"))
            : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}