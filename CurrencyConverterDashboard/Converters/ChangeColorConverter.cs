using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CurrencyConverterDashboard.Converters
{
    public class ChangeColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is decimal change)
            {
                return change switch
                {
                    > 0 => new SolidColorBrush(Colors.Green),
                    < 0 => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not SolidColorBrush brush) throw new InvalidOperationException("Unsupported brush color");
            if (brush.Color == Colors.Green)
            {
                return 1m; // Positive change
            }

            if (brush.Color == Colors.Red)
            {
                return -1m; // Negative change
            }

            if (brush.Color == Colors.Gray)
            {
                return 0m; // No change
            }
            throw new InvalidOperationException("Unsupported brush color");
        }
    }
}