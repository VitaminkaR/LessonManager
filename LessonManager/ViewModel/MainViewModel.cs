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
        [RelayCommand]
        private void OpenSideBar(ColumnDefinition param)
        {
            if (param != null)
                if (param.Width.Value == 256)
                {
                    param.Width = new System.Windows.GridLength(32);
                    SideBarOpenedElementVisibility = Visibility.Hidden;
                    SideBarClosedElementVisibility = Visibility.Visible;
                }
                else
                {
                    param.Width = new System.Windows.GridLength(256);
                    SideBarOpenedElementVisibility = Visibility.Visible;
                    SideBarClosedElementVisibility = Visibility.Hidden;
                }
        }

        [ObservableProperty]
        private bool m_IsSideBarOpen;

        [ObservableProperty]
        private Visibility m_SideBarOpenedElementVisibility;

        [ObservableProperty]
        private Visibility m_SideBarClosedElementVisibility;

        [ObservableProperty]
        private TreeViewItem m_ChoosenSubjectTreeItem;

        [RelayCommand]
        private void AddSubject()
        {
            new SubjectAddWindow().Show();
        }

        [RelayCommand]
        private void RemoveSubjectTreeElement()
        {
            if (ChoosenSubjectTreeItem == null)
            {
                MessageBox.Show("Не выбран ни один элемент");
                return;
            }

            // находим уровень дерева
            object levelObj = ChoosenSubjectTreeItem;
            int level = 0;
            while (levelObj.GetType() != typeof(TreeView))
            {
                level++;
                levelObj = ((TreeViewItem)levelObj).Parent;
            }

            switch (level)
            {
                // Удаление семестра
                case 1:
                    App.ApplicationContext.RemoveSemester(int.Parse((string)ChoosenSubjectTreeItem.Header));
                    break;
                // Удаление дисциплины
                case 2:
                    App.ApplicationContext.RemoveSubject((string)ChoosenSubjectTreeItem.Header, int.Parse((string)((TreeViewItem)ChoosenSubjectTreeItem.Parent).Header));
                    break;
                default:
                    break;
            }
        }

        public ObservableCollection<Subject> Subjects { get; set; }

        public MainViewModel()
        {
            SideBarOpenedElementVisibility = Visibility.Hidden;
            Subjects = App.ApplicationContext.Subjects.Local.ToObservableCollection();
        }
    }
}
