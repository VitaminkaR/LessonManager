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
            m_ViewModel.CurrentActivities.CollectionChanged += CurrentActivities_CollectionChanged;
        }

        private void CurrentActivities_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset && m_ViewModel.CurrentActivities.Count == 0)
                WorkFieldStackPanel.Children.Clear();
            if (!(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add))
                return;
            UpdateCurrentActivities(e.NewItems.Cast<Activity>().ToList());
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
            // получаем для каждого семестра названия предметов
            Dictionary<int, List<string>> semesterAndSubjects = new Dictionary<int, List<string>>();
            for (int i = 0; i < subjects.Count; i++)
            {
                int sem = subjects[i].SemesterNumber;
                if (!semesterAndSubjects.ContainsKey(sem)) semesterAndSubjects[sem] = new List<string>();
                semesterAndSubjects[sem].Add(subjects[i].Name);
            }

            // проходися по всем семестрам
            foreach (var key in semesterAndSubjects.Keys)
            {
                TreeViewItem treeViewItemSemesters = new TreeViewItem();
                treeViewItemSemesters.Header = key.ToString();

                // создание и установка контекстного меню для семестров
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem() { Header = "Удалить семестр" };
                menuItem.Click += m_ViewModel.RemoveSemesterElement;
                contextMenu.Items.Add(menuItem);
                treeViewItemSemesters.ContextMenu = contextMenu;

                List<string> list = semesterAndSubjects[key];
                // проходимся по всем дисциплинам
                for (int i = 0; i < list.Count; i++)
                {
                    TreeViewItem treeViewItemSubjects = new TreeViewItem();
                    treeViewItemSubjects.Header = list[i];

                    contextMenu = new ContextMenu();
                    menuItem = new MenuItem() { Header = "Удалить дисциплину" };
                    menuItem.Click += m_ViewModel.RemoveSubjectElement;
                    contextMenu.Items.Add(menuItem);
                    menuItem = new MenuItem() { Header = "Редактировать дисциплину" };
                    menuItem.Click += m_ViewModel.EditSubjectElement;
                    contextMenu.Items.Add(menuItem);
                    treeViewItemSubjects.ContextMenu = contextMenu;

                    contextMenu = new ContextMenu();
                    menuItem = new MenuItem() { Header = "Добавить занятие" };
                    menuItem.Click += m_ViewModel.AddActivity;
                    contextMenu.Items.Add(menuItem);
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

                    treeViewItemSemesters.Items.Add(treeViewItemSubjects);
                }
                SubjectsAndLabTreeView.Items.Add(treeViewItemSemesters);
            }
        }

        private void UpdateCurrentActivities(object param)
        {
            var activity = ((IList<Activity>)param)[0];

            StackPanel activityBlock = new StackPanel();

            Color stateColor;
            switch (activity.State)
            {
                case ActivityStateType.None: stateColor = Color.FromRgb(0, 0, 0); break;
                case ActivityStateType.Passed: stateColor = Color.FromRgb(0, 153, 0); break;
                case ActivityStateType.Ready: stateColor = Color.FromRgb(204, 204, 0); break;
                case ActivityStateType.Visited: stateColor = Color.FromRgb(0, 204, 204); break;
                case ActivityStateType.Answer: stateColor = Color.FromRgb(0, 153, 0); break;
                case ActivityStateType.NotVisited: stateColor = Color.FromRgb(204, 0, 0); break;
            }

            Button deleteActivityButton = new Button()
            {
                FontSize = 8,
                Content = "✖",
                Foreground = new SolidColorBrush(Color.FromRgb(255, 25, 25)),
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 16,
                Height = 16
            };
            deleteActivityButton.Click += (sender, e) => m_ViewModel.EditableActivity = activity;
            deleteActivityButton.Click += m_ViewModel.RemoveActivity;
            activityBlock.Children.Add(deleteActivityButton);

            TextBlock textBlock = new TextBlock()
            {
                FontSize = 14,
                Text = "Название занятия",
                Foreground = new SolidColorBrush(stateColor)
            };
            activityBlock.Children.Add(textBlock);

            TextBox nameTextBox = new TextBox()
            {
                FontSize = 12,
                Text = activity.Name
            };
            nameTextBox.TextChanged += (sender, e) =>
            {
                m_ViewModel.EditableActivity = activity;
                m_ViewModel.EditableActivityName = nameTextBox.Text;
                m_ViewModel.EditableActivityTime = activity.ActivityTime;
                m_ViewModel.EditableActivityState = activity.State;
            };
            nameTextBox.LostFocus += m_ViewModel.EditActivity;
            nameTextBox.KeyDown += (sender, e) => { if (e.Key == Key.Enter) deleteActivityButton.Focus(); };
            activityBlock.Children.Add(nameTextBox);

            textBlock = new TextBlock()
            {
                FontSize = 12,
                Text = $"Тип занятия: {activity.Type}"
            };
            activityBlock.Children.Add(textBlock);

            DatePicker datePicker = new DatePicker()
            {
                SelectedDate = activity.ActivityTime
            };
            datePicker.SelectedDateChanged += (sender, e) =>
            {
                m_ViewModel.EditableActivity = activity;
                m_ViewModel.EditableActivityName = activity.Name;
                m_ViewModel.EditableActivityTime = (DateTime)datePicker.SelectedDate;
                m_ViewModel.EditableActivityState = activity.State;
            };
            datePicker.SelectedDateChanged += m_ViewModel.EditActivity;
            activityBlock.Children.Add(datePicker);

            var enumlist = Enum.GetValues(typeof(ActivityStateType));
            ComboBox comboBox = new ComboBox()
            {
                ItemsSource = enumlist,
                SelectedItem = activity.State,
                Foreground = new SolidColorBrush(stateColor)
            };
            comboBox.SelectionChanged += (sender, e) =>
            {
                m_ViewModel.EditableActivity = activity;
                m_ViewModel.EditableActivityName = activity.Name;
                m_ViewModel.EditableActivityTime = activity.ActivityTime;
                m_ViewModel.EditableActivityState = (ActivityStateType)comboBox.SelectedItem;
            };
            comboBox.SelectionChanged += m_ViewModel.EditActivity;
            activityBlock.Children.Add(comboBox);

            WorkFieldStackPanel.Children.Add(activityBlock);
            WorkFieldStackPanel.Children.Add(new Rectangle() { Height = 4, StrokeThickness = 4, Stroke = new SolidColorBrush(Color.FromRgb(150, 150, 150)) });
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.ApplicationContext == null)
                throw new Exception("Ошибка загрузки контекста БД");

            App.ApplicationContext.Database.EnsureCreated();
            App.ApplicationContext.DBLoad();
        }

        private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            new SubjectAddWindow().Show();
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