using LessonManager.Model;
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
    /// Логика взаимодействия для SubjectAddWindow.xaml
    /// </summary>
    public partial class SubjectAddWindow : Window
    {
        public SubjectAddWindow()
        {
            InitializeComponent();

            ExamTypeComboBox.ItemsSource = Enum.GetValues(typeof(ExamType));
            Loaded += SubjectAddWindow_Loaded;
        }

        private void SubjectAddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ExamDatePicker.SelectedDate = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
