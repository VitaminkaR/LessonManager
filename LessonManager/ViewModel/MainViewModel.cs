using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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
        private RelayCommand<ColumnDefinition>? m_OpenSideBarCommand;
        public RelayCommand<ColumnDefinition> OpenSideBarCommand
        {
            get
            {
                return m_OpenSideBarCommand ??
                    (m_OpenSideBarCommand = new RelayCommand<ColumnDefinition>(param =>
                    {
                        if(param != null)
                            if(param.Width.Value == 256)
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
                    }));
            }
        }

        [ObservableProperty]
        private bool m_IsSideBarOpen;

        [ObservableProperty]
        private Visibility m_SideBarOpenedElementVisibility;

        [ObservableProperty]
        private Visibility m_SideBarClosedElementVisibility;

        public MainViewModel()
        {
            SideBarOpenedElementVisibility = Visibility.Hidden;
        }
    }
}
