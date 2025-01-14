using LessonManager.Model;
using LessonManager.ViewModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        private MainViewModel m_ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            m_ViewModel = new MainViewModel();
            DataContext = m_ViewModel;

            m_ViewModel.Subjects.CollectionChanged += Subjects_CollectionChanged;
        }

        private void Subjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Subject> subjects = (ObservableCollection<Subject>)sender;
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}