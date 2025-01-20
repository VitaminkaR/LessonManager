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
        }

        // все дисциплины
        public ObservableCollection<Subject> Subjects { get; set; }
        // выбранные по дисциплине занятия
        public ObservableCollection<Activity> CurrentActivities { get; set; }

        // редактируемая активность (должна устанавливаться перед вызовом метода удаления или редактирования занятия)
        public Activity EditableActivity { get; set; }
        public string? EditableActivityName { get; set; }
        public DateTime EditableActivityTime { get; set; }
        public ActivityStateType EditableActivityState { get; set; }

        // удаляет дисциплину из меню
        public void RemoveSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            App.ApplicationContext.SubjectDB.RemoveSubject((string)ChoosenSubjectTreeItem.Header, int.Parse((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header));
        }

        // удаляет целый семестр из меню
        public void RemoveSemesterElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            App.ApplicationContext.SubjectDB.RemoveSemester(int.Parse((string)ChoosenSubjectTreeItem.Header));
        }

        // редактирует дисциплину из меню
        public void EditSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            Subject s = App.ApplicationContext.SubjectDB.GetSubject((string)ChoosenSubjectTreeItem.Header, int.Parse((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header));
            new SubjectEditWindow(s).Show();
        }

        // добаляет занятие в меню
        public void AddActivity(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }

            string type = (string)ChoosenSubjectTreeItem.Header;
            ActivityType activityType = (ActivityType)Enum.Parse(typeof(ActivityType), type);
            if (!Enum.IsDefined(typeof(ActivityType), activityType))
            {
                MessageBox.Show("Выбран неправильный элемент! Выберите категорию занятий.");
                return;
            }

            TreeViewItem SubjectTreeViewItem = (TreeViewItem)ChoosenSubjectTreeItem.Parent;
            TreeViewItem SemesterTreeViewItem = (TreeViewItem)SubjectTreeViewItem.Parent;
            // получем дисциплину этого занятия
            string subjectName = SubjectTreeViewItem.Header.ToString();
            int semesterNumber = int.Parse(SemesterTreeViewItem.Header.ToString());
            Subject subject = App.ApplicationContext.SubjectDB.GetSubject(subjectName, semesterNumber);

            new ActivityAddWindow(activityType, subject).Show();
        }

        // инициализирует установку занятий (получает все необхоимые данные)
        public void SetActivities(object? sender, RoutedEventArgs e)
        {
            TreeViewItem curEl = (TreeViewItem)sender;
            string type = curEl.Header.ToString();
            ActivityType activityType = (ActivityType)Enum.Parse(typeof(ActivityType), type);

            TreeViewItem SubjectTreeViewItem = (TreeViewItem)curEl.Parent;
            TreeViewItem SemesterTreeViewItem = (TreeViewItem)SubjectTreeViewItem.Parent;
            // получем дисциплину этого занятия
            string subjectName = SubjectTreeViewItem.Header.ToString();
            int semesterNumber = int.Parse(SemesterTreeViewItem.Header.ToString());
            Subject subject = App.ApplicationContext.SubjectDB.GetSubject(subjectName, semesterNumber);

            SettingActivities(subject, activityType);
        }

        // редактирует событие
        public void EditActivity(object? sender, RoutedEventArgs e)
        {
            if (EditableActivity == null)
                MessageBox.Show("Ошибка выбора занятия");

            if (EditableActivityName == null)
                MessageBox.Show("Ошибка редатирования");

            App.ApplicationContext.ActivityDB.EditActivity(
                EditableActivity.Name,
                EditableActivity.Subject,
                EditableActivityName,
                EditableActivity.Type,
                EditableActivityTime,
                EditableActivityState
                );

            SettingActivities(EditableActivity.Subject, EditableActivity.Type);

            EditableActivity = null;
        }

        // удаляет событие
        public void RemoveActivity(object? sender, RoutedEventArgs e)
        {
            if (EditableActivity == null)
                MessageBox.Show("Ошибка выбора занятия");
            App.ApplicationContext.ActivityDB.RemoveActivity(EditableActivity.Name, EditableActivity.Subject);
            CurrentActivities.Remove(EditableActivity);

            SettingActivities(EditableActivity.Subject, EditableActivity.Type);

            EditableActivity = null;
        }

        // непосредственно обновляет коллекцию занятий
        private void SettingActivities(Subject subject, ActivityType type)
        {
            var activities = App.ApplicationContext.ActivityDB.GetAllActivitiesOfTypeFromSubject(subject, type);
            CurrentActivities.Clear();
            foreach (var item in activities)
                CurrentActivities.Add(item);
        }

        public MainViewModel()
        {
            Subjects = App.ApplicationContext.Subjects.Local.ToObservableCollection();
            CurrentActivities = new ObservableCollection<Activity>();

            m_ISCImport = new OGUICSImport();
        }
    }
}
