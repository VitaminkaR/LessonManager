using LessonManager.Model;
using LessonManager.ViewModel;
using System.Windows;

namespace LessonManager.View
{
    /// <summary>
    /// Логика взаимодействия для SubjectEditWindow.xaml
    /// </summary>
    public partial class SubjectEditWindow : Window
    {
        internal SubjectEditWindow(Subject subject)
        {
            InitializeComponent();

            ExamDatePicker.SelectedDate = DateTime.Now;
            ExamTypeComboBox.ItemsSource = Enum.GetValues(typeof(ExamType));
            SemesterComboBox.ItemsSource = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            DataContext = new SubjectEditViewModel(subject);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
