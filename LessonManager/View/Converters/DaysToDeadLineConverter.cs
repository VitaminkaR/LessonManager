using System.Globalization;
using System.Windows.Data;

namespace LessonManager.View.Converters
{
    class DaysToDeadLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt1 = (DateTime)value;
            var dt2 = DateTime.Now;
            return (new DateTime(dt1.Year, dt1.Month, dt1.Day) - new DateTime(dt2.Year, dt2.Month, dt2.Day)).Days;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
