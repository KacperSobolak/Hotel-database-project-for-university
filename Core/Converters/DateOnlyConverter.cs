using System.Globalization;
using System.Windows.Data;

namespace Hotel.Core.Converters
{
    public class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue);  // Convert DateOnly to DateTime
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);  // Convert DateTime back to DateOnly
            }
            return value;
        }
    }
}
