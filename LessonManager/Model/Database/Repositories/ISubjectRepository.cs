using LessonManager.Model.Database.Entities;

namespace LessonManager.Model.Database.Repositories
{
    internal interface ISubjectRepository
    {
        Task AddAsync(string name, string exam, DateTime dateTime);
        Task AddAsync(SubjectEntity subject);
        Task EditAsync(string lastname, string name, string exam, DateTime dateTime);
        Task<IEnumerable<SubjectEntity>> GetAsync();
        Task<SubjectEntity> GetAsync(string name);
        Task RemoveAsync(string name);
    }
}