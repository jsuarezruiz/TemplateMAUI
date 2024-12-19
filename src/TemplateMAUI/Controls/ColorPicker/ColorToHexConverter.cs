using System.Globalization;

namespace TemplateMAUI.Controls
{
    public class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;

            if (value is Color valueColor)
            {
                color = valueColor;
            }
            else if (value is SolidColorBrush valueBrush)
            {
                color = valueBrush.Color;
            }
            else
            {
                // Invalid color value provided
                return BindableProperty.UnsetValue;
            }
   
            string hexColor = color.ToHex();

            return hexColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hexValue = value.ToString()!;

            if (hexValue.StartsWith("#"))
            {
                try
                {
                    return Color.FromRgba(hexValue);
                }
                catch
                {
                    // Invalid hex color value provided
                    return BindableProperty.UnsetValue;
                }
            }
            else
            {
                try
                {
                    return Color.FromArgb("#" + hexValue);
                }
                catch
                {
                    // Invalid hex color value provided
                    return BindableProperty.UnsetValue;
                }
            }
        }
    }
}
