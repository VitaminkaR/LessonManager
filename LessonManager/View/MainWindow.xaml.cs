using LessonManager.Model;
using LessonManager.ViewModel;
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
            Loaded += MainWindow_Loaded;
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
            ObservableCollection<Subject> subjects = (ObservableCollection<Subject>)param;
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.ApplicationContext == null)
                throw new Exception("Ошибка загрузки контекста БД");

            App.ApplicationContext.Database.EnsureCreated();
            App.ApplicationContext.DBLoad();
        }

        private void InfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string msg = $"Разработчик: Владимир Рыбянцов\n" +
                                $"Версия:{App.VERSION}";
            MessageBox.Show(msg, "Справка", MessageBoxButton.OK);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}