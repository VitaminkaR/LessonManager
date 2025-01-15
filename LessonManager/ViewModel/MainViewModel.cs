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
using System.Xml.Linq;

namespace LessonManager.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool m_IsSideBarOpen;

        [ObservableProperty]
        private TreeViewItem m_ChoosenSubjectTreeItem;

        [RelayCommand]
        private void RemoveSubjectTreeElement()
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }

            int level = GetTreeLevel(ChoosenSubjectTreeItem);

            switch (level)
            {
                // Удаление семестра
                case 1:
                    App.ApplicationContext.SubjectDB.RemoveSemester(int.Parse((string)ChoosenSubjectTreeItem.Header));
                    break;
                // Удаление дисциплины
                case 2:
                    App.ApplicationContext.SubjectDB.RemoveSubject((string)ChoosenSubjectTreeItem.Header, int.Parse((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header));
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void EditSubjectTreeElement()
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }

            int level = GetTreeLevel(ChoosenSubjectTreeItem);

            switch (level)
            {
                // Удаление семестра
                case 1:
                    MessageBox.Show("Нельзя редактировать семестр");
                    break;
                // Удаление дисциплины
                case 2:
                    Subject s = App.ApplicationContext.SubjectDB.GetSubject((string)ChoosenSubjectTreeItem.Header, int.Parse((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header));
                    new SubjectEditWindow(s).Show();
                    break;
                default:
                    break;
            }
        }

        public ObservableCollection<Subject> Subjects { get; set; }

        private int GetTreeLevel(object levelObj)
        {
            // находим уровень дерева
            int level = 0;
            while (levelObj.GetType() != typeof(TreeView))
            {
                level++;
                levelObj = ((TreeViewItem)levelObj).Parent;
            }

            return level;
        }

        public MainViewModel()
        {
            Subjects = App.ApplicationContext.SubjectDB.Subjects.Local.ToObservableCollection();
        }
    }
}
