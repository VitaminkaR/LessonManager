using LessonManager.Model.Database;
using LessonManager.Model.Database.Entities;
using LessonManager.Model.Database.Repositories;
using LessonManager.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LessonManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel m_ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            m_ViewModel = new MainViewModel();
            DataContext = m_ViewModel;

            this.Loaded += MainWindow_Loaded;
        }

        private void SubjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source is ListView)
                m_ViewModel.SetActivities();
        }

        private void ActivitiesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source is TabControl)
                m_ViewModel.SetActivities();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            m_ViewModel.SetSubjects();
            ActivitiesTab.SelectionChanged += ActivitiesTab_SelectionChanged;
            SubjectsListView.SelectionChanged += SubjectsListView_SelectionChanged;
        }

        private void InfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string msg = $"Разработчик: Владимир Рыбянцов\n" +
                                $"Версия:{App.CurrentApplication.Configuration["VERSION"]}";
            MessageBox.Show(msg, "Справка", MessageBoxButton.OK);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}