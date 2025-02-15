using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model.Database.Entities;
using LessonManager.Model.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LessonManager.ViewModel
{
    internal partial class SubjectEditViewModel : ObservableObject
    {
        private string m_PredName;

        [ObservableProperty]
        private string? m_SubjectName;

        [ObservableProperty]
        private string? m_ExamType;

        [ObservableProperty]
        private DateTime m_ExamDate;

        [RelayCommand]
        private void EditSubject()
        {
            EditSubjectAsync();
        }

        private async void EditSubjectAsync()
        {
            var serviceProvider = App.CurrentApplication.services.BuildServiceProvider();
            var subjects = serviceProvider.GetRequiredService<ISubjectRepository>();

            await subjects.EditAsync(m_PredName, SubjectName, ExamType, ExamDate);
        }

        public SubjectEditViewModel(SubjectEntity subject)
        {
            SubjectName = subject.Name;
            ExamType= subject.Exam.ToString();
            ExamDate = subject.ExamDate;
            m_PredName = subject.Name;
        }
    }
}
