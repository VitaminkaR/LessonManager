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
            SemesterComboBox.ItemsSource = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
