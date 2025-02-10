using CommunityToolkit.Mvvm.ComponentModel;
using LessonManager.Model;

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

        public StatisticsViewModel()
        {
            m_StatisticsManager = new StatisticsManager(App.ApplicationContext.Subjects);
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
