using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.ViewModel
{
    internal partial class SubjectAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? m_SubjectName;

        [ObservableProperty]
        private string? m_SemestrNubmer;

        [ObservableProperty]
        private string? m_ExamType;

        [ObservableProperty]
        private DateTime m_ExamDate;

        [RelayCommand]
        private void AddSubject()
        {
            if (m_ExamType == null || m_SubjectName == null || m_SemestrNubmer == null)
            {
                MessageBox.Show("Не все поля заполненные");
                return;
            }

            App.ApplicationContext?.AddSubject(SubjectName, int.Parse(m_SemestrNubmer), ExamType, ExamDate);
        }
    }
}
