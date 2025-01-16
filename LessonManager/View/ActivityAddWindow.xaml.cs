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
    /// Логика взаимодействия для ActivityAddWindow.xaml
    /// </summary>
    public partial class ActivityAddWindow : Window
    {
        private ActivityType m_ActivityType;
        private Subject m_Subject;

        internal ActivityAddWindow(ActivityType activityType, Subject subject)
        {
            InitializeComponent();

            m_ActivityType = activityType;
            Loaded += ActivityAddWindow_Loaded;
            m_Subject = subject;

            DataContext = new ActivityAddViewModel();
        }

        private void ActivityAddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ActivityTypeTextBlock.Text = m_ActivityType.ToString();
            ActivityAddViewModel vm = (ActivityAddViewModel)DataContext;
            vm.ActivityType = m_ActivityType.ToString();
            vm.SubjectActivity = m_Subject;
            vm.AutoSetName();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
