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
            AddSubjectAsync();
        }

        private async void AddSubjectAsync()
        {
            if (ExamType == null || SubjectName == null)
            {
                MessageBox.Show("Не все поля заполненные");
                return;
            }

            var serviceProvider = App.CurrentApplication.services.BuildServiceProvider();
            var subjects = serviceProvider.GetRequiredService<ISubjectRepository>();

            if (subjects.GetAsync().Result.Any((s) => s.Name == SubjectName))
            {
                MessageBox.Show("Дисциплина с таким названием уже существует");
                return;
            }

            await subjects.AddAsync(SubjectName, ExamType, ExamDate);
        }
    }
}
