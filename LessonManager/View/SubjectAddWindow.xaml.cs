using LessonManager.Core.Enums;
using LessonManager.Model;
using System.Windows;

namespace LessonManager.View
{
    /// <summary>
    /// Логика взаимодействия для SubjectAddWindow.xaml
    /// </summary>
    public partial class SubjectAddWindow : Window
    {
        public SubjectAddWindow()
        {
            InitializeComponent();

            ExamDatePicker.SelectedDate = DateTime.Now;
            ExamTypeComboBox.ItemsSource = Enum.GetValues(typeof(ExamType));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
