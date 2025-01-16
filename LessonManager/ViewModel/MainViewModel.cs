using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model;
using LessonManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace LessonManager.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool m_IsSideBarOpen;

        [ObservableProperty]
        private TreeViewItem m_ChoosenSubjectTreeItem;

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

            string type = (string)m_ChoosenSubjectTreeItem.Header;
            ActivityType activityType = (ActivityType)Enum.Parse(typeof(ActivityType), type);
            if(!Enum.IsDefined(typeof(ActivityType), activityType))
            {
                MessageBox.Show("Выбран неправильный элемент! Выберите категорию занятий.");
                return;
            }

            // получем дисциплину этого занятия
            string subjectName = (string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header;
            int semesterNumber = int.Parse((string)((TreeViewItem)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Parent).Header);
            Subject subject = App.ApplicationContext.SubjectDB.GetSubject(subjectName, semesterNumber);
            
            new ActivityAddWindow(activityType, subject).Show();
        }

        // устанавливает выбранные занятия
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

            var activities = App.ApplicationContext.ActivityDB.GetAllActivitiesOfTypeFromSubject(subject, activityType);
            CurrentActivities.Clear();
            foreach (var item in activities)
                CurrentActivities.Add(item);
        }

        public MainViewModel()
        {
            Subjects = App.ApplicationContext.Subjects.Local.ToObservableCollection();
            CurrentActivities = new ObservableCollection<Activity>();
        }
    }
}
