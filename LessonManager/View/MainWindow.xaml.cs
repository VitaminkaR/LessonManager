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

            m_ViewModel.Subjects.CollectionChanged += Subjects_CollectionChanged;
            ActivitiesTreeView.SelectedItemChanged += ActivitiesTreeView_SelectedItemChanged;
        }

        private void ActivitiesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            m_ViewModel.ChoosenSubjectTreeItem = (TreeViewItem)ActivitiesTreeView.SelectedItem;
        }

        private void Subjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSubjectAndActivityView(sender);
        }

        private void UpdateSubjectAndActivityView(object param)
        {
            ObservableCollection<SubjectEntity> subjects = (ObservableCollection<SubjectEntity>)param;
            ActivitiesTreeView.Items.Clear();

            // проходимся по всем дисциплинам
            for (int i = 0; i < subjects.Count; i++)
            {
                TreeViewItem treeViewItemSubjects = new TreeViewItem();
                treeViewItemSubjects.Header = subjects[i].Name;

                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem() { Header = "Удалить дисциплину" };
                menuItem.Click += m_ViewModel.RemoveSubjectElement;
                contextMenu.Items.Add(menuItem);
                menuItem = new MenuItem() { Header = "Редактировать дисциплину" };
                menuItem.Click += m_ViewModel.EditSubjectElement;
                contextMenu.Items.Add(menuItem);
                treeViewItemSubjects.ContextMenu = contextMenu;

                // категории
                TreeViewItem treeViewItemCategory = new TreeViewItem() { Header = "Lab", ContextMenu = contextMenu };
                treeViewItemCategory.Selected += m_ViewModel.SetActivities;
                treeViewItemSubjects.Items.Add(treeViewItemCategory);
                treeViewItemCategory = new TreeViewItem() { Header = "Prac", ContextMenu = contextMenu };
                treeViewItemCategory.Selected += m_ViewModel.SetActivities;
                treeViewItemSubjects.Items.Add(treeViewItemCategory);
                treeViewItemCategory = new TreeViewItem() { Header = "Lec", ContextMenu = contextMenu };
                treeViewItemCategory.Selected += m_ViewModel.SetActivities;
                treeViewItemSubjects.Items.Add(treeViewItemCategory);

                ActivitiesTreeView.Items.Add(treeViewItemSubjects);
            }
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