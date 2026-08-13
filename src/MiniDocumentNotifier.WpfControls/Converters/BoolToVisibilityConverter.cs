using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MiniDocumentNotifier.WpfControls.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value != null && (bool)value;
            var invert = string.Equals(parameter as string, "Invert", StringComparison.OrdinalIgnoreCase);

            if (invert) boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = value is Visibility visibility && visibility == Visibility.Visible;
            var invert = string.Equals(parameter as string, "Invert", StringComparison.OrdinalIgnoreCase);

            return invert ? !isVisible : isVisible;
        }
    }
}