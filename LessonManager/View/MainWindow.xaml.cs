using LessonManager.Model;
using LessonManager.ViewModel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LessonManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double SIDE_BAR_OPEN_SIZE = 256;
        const double SIDE_BAR_CLOSE_SIZE = 32;

        private MainViewModel m_ViewModel;

        private bool m_IsSideBarOpened;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            m_ViewModel = new MainViewModel();
            DataContext = m_ViewModel;

            m_ViewModel.Subjects.CollectionChanged += Subjects_CollectionChanged;
            SubjectsAndLabTreeView.SelectedItemChanged += SubjectsAndLabTreeView_SelectedItemChanged;

            // настройка параметров бокового меню
            m_IsSideBarOpened = false;
            AddActivityTextBlock.Visibility = Visibility.Hidden;
            AddSubjectTextBlock.Visibility = Visibility.Hidden;
            SubjectsAndLabTreeView.Visibility = Visibility.Hidden;
        }

        private void SubjectsAndLabTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            m_ViewModel.ChoosenSubjectTreeItem = (TreeViewItem)SubjectsAndLabTreeView.SelectedItem;
        }

        private void Subjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSubjectAndActivityView(sender);
        }

        private void UpdateSubjectAndActivityView(object param)
        {
            ObservableCollection<Subject> subjects = (ObservableCollection<Subject>)param;
            SubjectsAndLabTreeView.Items.Clear();
            Dictionary<int, List<string>> semesterAndSubjects = new Dictionary<int, List<string>>();
            for (int i = 0; i < subjects.Count; i++)
            {
                int sem = subjects[i].SemesterNumber;
                if (!semesterAndSubjects.ContainsKey(sem)) semesterAndSubjects[sem] = new List<string>();
                semesterAndSubjects[sem].Add(subjects[i].Name);
            }

            foreach (var key in semesterAndSubjects.Keys)
            {
                TreeViewItem treeViewItemSemesters = new TreeViewItem();
                treeViewItemSemesters.Header = key.ToString();
                List<string> list = semesterAndSubjects[key];
                for (int i = 0; i < list.Count; i++)
                {
                    TreeViewItem treeViewItemSubjects = new TreeViewItem();
                    treeViewItemSubjects.Header = list[i];

                    treeViewItemSubjects.Items.Add(new TreeViewItem() { Header = "Lab" });
                    treeViewItemSubjects.Items.Add(new TreeViewItem() { Header = "Prac" });
                    treeViewItemSubjects.Items.Add(new TreeViewItem() { Header = "Lec" });

                    treeViewItemSemesters.Items.Add(treeViewItemSubjects);
                }
                SubjectsAndLabTreeView.Items.Add(treeViewItemSemesters);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.ApplicationContext == null)
                throw new Exception("Ошибка загрузки контекста БД");

            App.ApplicationContext.Database.EnsureCreated();
            App.ApplicationContext.DBLoad();
        }

        private void SideBarOpen()
        {
            m_IsSideBarOpened = true;
            AddActivityTextBlock.Visibility = Visibility.Visible;
            AddSubjectTextBlock.Visibility = Visibility.Visible;
            SubjectsAndLabTreeView.Visibility = Visibility.Visible;
            SideBar.Width = new GridLength(SIDE_BAR_OPEN_SIZE);
        }

        private void SideBarClose()
        {
            m_IsSideBarOpened = false;
            AddActivityTextBlock.Visibility = Visibility.Hidden;
            AddSubjectTextBlock.Visibility = Visibility.Hidden;
            SubjectsAndLabTreeView.Visibility = Visibility.Hidden;
            SideBar.Width = new GridLength(SIDE_BAR_CLOSE_SIZE);
        }

        private void SideBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_IsSideBarOpened)
                SideBarClose();
            else
                SideBarOpen();
        }

        private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            new SubjectAddWindow().Show();
        }
    }
}