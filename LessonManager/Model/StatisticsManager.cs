using Microsoft.EntityFrameworkCore;

namespace LessonManager.Model
{
    internal class StatisticsManager
    {
        private readonly DbSet<Subject> m_Subjects;

        public StatisticsManager(DbSet<Subject> subjects)
        {
            this.m_Subjects = subjects;
        }

        public async Task<int> GetLectionsCount() => 
            await m_Subjects.Select((s) => s.Activities.Where((a) => a.Type == ActivityType.Lec).Count()).SumAsync();

        public async Task<int> GetVisitedLectionsCount() =>
            await m_Subjects.Select((s) => s.Activities.Where((a) => a.Type == ActivityType.Lec && a.State == ActivityStateType.Visited).Count()).SumAsync();

        public async Task<int> GetPracticeCount() =>
            await m_Subjects.Select((s) => s.Activities.Where((a) => a.Type == ActivityType.Prac).Count()).SumAsync();

        public async Task<int> GetLabsCount() =>
            await m_Subjects.Select((s) => s.Activities.Where((a) => a.Type == ActivityType.Lab).Count()).SumAsync();

        public async Task<int> GetPassedLabsCount() =>
            await m_Subjects.Select((s) => s.Activities.Where((a) => a.Type == ActivityType.Lab && a.State == ActivityStateType.Passed).Count()).SumAsync();
    }
}
