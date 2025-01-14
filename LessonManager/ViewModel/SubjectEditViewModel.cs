using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LessonManager.ViewModel
{
    internal partial class SubjectEditViewModel : ObservableObject
    {
        private string m_PredName;
        private int m_PredSemesterNumber;

        [ObservableProperty]
        private string? m_SubjectName;

        [ObservableProperty]
        private string? m_SemestrNubmer;

        [ObservableProperty]
        private string? m_ExamType;

        [ObservableProperty]
        private DateTime m_ExamDate;

        [RelayCommand]
        private void EditSubject()
        {
            App.ApplicationContext.EditSubject(m_PredName, m_PredSemesterNumber, SubjectName, int.Parse(m_SemestrNubmer), ExamType, ExamDate);
        }

        public SubjectEditViewModel(Subject subject)
        {
            SubjectName = subject.Name;
            SemestrNubmer = subject.SemesterNumber.ToString();
            ExamType= subject.Exam.ToString();
            ExamDate = subject.DateTime;
            m_PredName = subject.Name;
            m_PredSemesterNumber = subject.SemesterNumber;
        }
    }
}
