using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model;
using LessonManager.View;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LessonManager.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        private IISCImport m_ISCImport;

        [ObservableProperty]
        private TreeViewItem m_ChoosenSubjectTreeItem;

        [RelayCommand]
        private void OpenISCFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".ics";
            dialog.Filter = "Файл календаря(.ics)|*.ics";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                    m_ISCImport.Init(fs);

                ICollection<Subject> subjects = m_ISCImport.GetSubjects();

                foreach (Subject subject in subjects)
                {
                    App.ApplicationContext?.SubjectDB.AddSubject(subject);
                }
            }
        }

        [RelayCommand]
        private void ReloadDB()
        {
            MessageBoxResult rsltMessageBox = MessageBox.Show(
                "Вы действительно хотите полностью удалить информацию о занятиях?",
                "Потверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    App.ApplicationContext.ClearDB();
                    break;
            }

            CurrentActivities.Clear();
        }

        [RelayCommand]
        private void AddSubject()
        {
            new SubjectAddWindow().Show();
        }

        [RelayCommand]
        private void GetStatistics()
        {
            new StatisticsWindow().Show();
        }

        // все дисциплины
        public ObservableCollection<Subject> Subjects { get; set; }
        // выбранные по дисциплине занятия
        public ObservableCollection<Activity> CurrentActivities { get; set; }

        // удаляет дисциплину из меню
        public void RemoveSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            App.ApplicationContext.SubjectDB.RemoveSubject((string)ChoosenSubjectTreeItem.Header);
        }

        // редактирует дисциплину из меню
        public void EditSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            Subject s = App.ApplicationContext.SubjectDB.GetSubject((string)ChoosenSubjectTreeItem.Header);
            new SubjectEditWindow(s).Show();
        }

        // инициализирует установку занятий (получает все необхоимые данные)
        public void SetActivities(object? sender, RoutedEventArgs e)
        {
            TreeViewItem curEl = (TreeViewItem)sender;
            string type = curEl.Header.ToString();
            ActivityType activityType = (ActivityType)Enum.Parse(typeof(ActivityType), type);

            TreeViewItem SubjectTreeViewItem = (TreeViewItem)curEl.Parent;
            // получем дисциплину этого занятия
            string subjectName = SubjectTreeViewItem.Header.ToString();
            Subject subject = App.ApplicationContext.SubjectDB.GetSubject(subjectName);

            SettingActivities(subject, activityType);
        }

        // непосредственно обновляет коллекцию занятий
        private void SettingActivities(Subject subject, ActivityType type)
        {
            var activities = App.ApplicationContext.ActivityDB.GetAllActivitiesOfTypeFromSubject(subject, type);
            foreach (var item in CurrentActivities)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }
            CurrentActivities.Clear();
            foreach (var item in activities)
            {
                CurrentActivities.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        // редактирвоание занятия
        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.ApplicationContext.ActivityDB.EditActivity((Activity)sender);
        }

        public MainViewModel()
        {
            Subjects = App.ApplicationContext.Subjects.Local.ToObservableCollection();
            CurrentActivities = new ObservableCollection<Activity>();
            CurrentActivities.CollectionChanged += CurrentActivities_CollectionChanged;

            m_ISCImport = new OGUICSImport();
        }

        private void CurrentActivities_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // удаление активности
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                Activity activity = (Activity)e.OldItems[0];
                App.ApplicationContext.ActivityDB.RemoveActivity(activity);
            }
            // добавление активности
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems.Count == 1)
            {
                Activity activity = (Activity)e.NewItems[0];
                if(activity.Name == null)
                {
                    if (ChoosenSubjectTreeItem == null)
                    {
                        MessageBox.Show("Не выбран ни один элемент");
                        return;
                    }
                    string type = (string)ChoosenSubjectTreeItem.Header;
                    ActivityType activityType = (ActivityType)Enum.Parse(typeof(ActivityType), type);

                    Subject s = App.ApplicationContext.SubjectDB.GetSubject((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header);

                    activity.Name = "";
                    activity.Subject = s;
                    activity.Type = activityType;
                    activity.ActivityTime = DateTime.Now;
                    App.ApplicationContext.ActivityDB.AddActivity(activity);

                    activity.PropertyChanged += Item_PropertyChanged;
                }
            }
        }
    }
}
