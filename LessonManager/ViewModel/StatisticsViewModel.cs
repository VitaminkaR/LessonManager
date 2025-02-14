using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LessonManager.Model;
using LessonManager.Model.Database.Repositories;
using LessonManager.Model.Database;
using Microsoft.Extensions.DependencyInjection;

namespace LessonManager.ViewModel
{
    internal partial class StatisticsViewModel : ObservableObject
    {
        private readonly StatisticsManager m_StatisticsManager;

        [ObservableProperty]
        private int m_LectionsCount;

        [ObservableProperty]
        private int m_VisitedLectionsCount;

        [ObservableProperty]
        private int m_PracticeCount;

        [ObservableProperty]
        private int m_LabsCount;

        [ObservableProperty]
        private int m_PassedLabsCount;

        [RelayCommand]
        private void Update() => Analyse();

        public StatisticsViewModel()
        {
            var serviceProvider = App.CurrentApplication.services.BuildServiceProvider();
            var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
            m_StatisticsManager = new StatisticsManager(applicationContext.Subjects);
            Analyse();
        }

        private async Task Analyse()
        {
            LectionsCount = await m_StatisticsManager.GetLectionsCount();
            VisitedLectionsCount = await m_StatisticsManager.GetVisitedLectionsCount();
            PracticeCount = await m_StatisticsManager.GetPracticeCount();
            LabsCount = await m_StatisticsManager.GetLabsCount();
            PassedLabsCount = await m_StatisticsManager.GetPassedLabsCount();
        }
    }
}
