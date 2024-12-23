using System.Globalization;

namespace TemplateMAUI.Controls
{
    public class ColorToRgbaConverter : IValueConverter
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

            if (parameter is not string paramString)
                return BindableProperty.UnsetValue;

            string result = string.Empty;

            if (paramString == "Red")
                result = (color.Red * 255).ToString("F0", CultureInfo.InvariantCulture);

            if (paramString == "Green")
                result = (color.Green * 255).ToString("F0", CultureInfo.InvariantCulture);

            if (paramString == "Blue")
                result = (color.Blue * 255).ToString("F0", CultureInfo.InvariantCulture);

            if (paramString == "Alpha")
                result = (color.Alpha * 255).ToString("F0", CultureInfo.InvariantCulture);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}