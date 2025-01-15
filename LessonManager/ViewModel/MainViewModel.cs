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

        public void RemoveSubjectElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            App.ApplicationContext.SubjectDB.RemoveSubject((string)ChoosenSubjectTreeItem.Header, int.Parse((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header));
        }

        public void RemoveSemesterElement(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
            App.ApplicationContext.SubjectDB.RemoveSemester(int.Parse((string)ChoosenSubjectTreeItem.Header));
        }

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

        public void AddActivity(object? sender, RoutedEventArgs e)
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }
        }

        public ObservableCollection<Subject> Subjects { get; set; }

        public MainViewModel()
        {
            Subjects = App.ApplicationContext.Subjects.Local.ToObservableCollection();
        }
    }
}
