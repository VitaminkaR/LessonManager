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

            DataContext = new SubjectEditViewModel(subject);
            ExamTypeComboBox.SelectedItem = subject.Exam;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
