using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LessonManager.View.Converters
{
    class DaysToDeadLineColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt1 = (DateTime)value;
            var dt2 = DateTime.Now;
            int days = (new DateTime(dt1.Year, dt1.Month, dt1.Day) - new DateTime(dt2.Year, dt2.Month, dt2.Day)).Days;
            return new SolidColorBrush(days > 0 ? Color.FromRgb(0, 200, 0) : (days == 0 ? Color.FromRgb(200, 200, 0) : Color.FromRgb(200, 0, 0)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
