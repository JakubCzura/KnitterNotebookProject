using KnitterNotebook.Models.Dtos;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KnitterNotebook.Converters.XAMLConverters
{
    /// <summary>
    /// Converts <see cref="SampleDto"/>'s NeedleSize and NeedleSizeUnit to a string. Works in Mode=OneWay only.
    /// </summary>
    public class SampleDtoNeedleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is SampleDto sample ? $"{sample?.NeedleSize} {sample?.NeedleSizeUnit}" : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }
}