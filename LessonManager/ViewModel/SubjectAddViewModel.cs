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
            if (ExamType == null || SubjectName == null || SemestrNubmer == null)
            {
                MessageBox.Show("Не все поля заполненные");
                return;
            }

            App.ApplicationContext?.SubjectDB.AddSubject(SubjectName, int.Parse(SemestrNubmer), ExamType, ExamDate);
        }
    }
}
