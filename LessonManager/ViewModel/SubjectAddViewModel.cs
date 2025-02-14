using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model.Database.Repositories;
using LessonManager.Model.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LessonManager.ViewModel
{
    internal partial class SubjectAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? m_SubjectName;

        [ObservableProperty]
        private string? m_ExamType;

        [ObservableProperty]
        private DateTime m_ExamDate;

        [RelayCommand]
        private void AddSubject()
        {
            if (ExamType == null || SubjectName == null)
            {
                MessageBox.Show("Не все поля заполненные");
                return;
            }

            var serviceProvider = App.CurrentApplication.services.BuildServiceProvider();
            var subjects = serviceProvider.GetRequiredService<ISubjectRepository>();

            subjects.AddSubject(SubjectName, ExamType, ExamDate);
        }
    }
}
