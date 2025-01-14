using LessonManager.Model;
using LessonManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
