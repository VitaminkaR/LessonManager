using LessonManager.Core.Enums;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LessonManager.View.Converters
{
    public class ActivityStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActivityStateType state)
            {
                if(state == ActivityStateType.None) return Brushes.Transparent;
                return new SolidColorBrush(GetActivityStateColor(state));
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color GetActivityStateColor(ActivityStateType state)
        {
            switch (state)
            {
                case ActivityStateType.None: return Color.FromRgb(0, 0, 0);
                case ActivityStateType.Passed: return Color.FromRgb(0, 153, 0);
                case ActivityStateType.Ready: return Color.FromRgb(204, 204, 0);
                case ActivityStateType.Visited: return Color.FromRgb(0, 204, 204);
                case ActivityStateType.Answer: return Color.FromRgb(0, 153, 0);
                case ActivityStateType.NotVisited: return Color.FromRgb(204, 0, 0);
            }

            return Color.FromRgb(0, 0, 0);
        }

    }
}
