using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LessonManager.View.Converters
{
    class DaysToDeadLineColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int days = (((DateTime)value) - DateTime.Now).Days;
            return new SolidColorBrush(days > 0 ? Color.FromRgb(0, 200, 0) : (days == 0 ? Color.FromRgb(200, 200, 0) : Color.FromRgb(200, 0, 0)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
