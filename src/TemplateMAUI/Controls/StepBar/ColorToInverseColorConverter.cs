using System.Globalization;
namespace TemplateMAUI.Controls
{
    public class ColorToInverseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertColorToInverseColor(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private Color ConvertColorToInverseColor(object value)
        {
            if (value != null && value is Color color)
            {
                return new Color(1 - color.Red, 1 - color.Green, 1 - color.Blue);
            }

            throw new ArgumentException("Expected value to be a type of Color", nameof(value));
        }
    }
}