using System.Globalization;
using System.Windows.Data;

namespace LessonManager.View.Converters
{
    class DaysToDeadLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((DateTime)value) - DateTime.Now).Days;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
