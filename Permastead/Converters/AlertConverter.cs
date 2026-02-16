using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Permastead.Converters;

public class AlertConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int alertLevel)
        {
            if (alertLevel <= 2)
                return new SolidColorBrush(Colors.Maroon, 0.6);
            else if (alertLevel <= 1)
                return new SolidColorBrush(Colors.DarkGoldenrod, 0.6);
            else
                return new SolidColorBrush(Colors.DarkGray, 0.6);
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}