using LessonManager.Model.Database.Entities;

namespace LessonManager.Model.Database.Repositories
{
    internal interface ISubjectRepository
    {
        void AddSubject(string name, string exam, DateTime dateTime);
        void AddSubject(SubjectEntity subject);
        void EditSubject(string lastname, string name, string exam, DateTime dateTime);
        Task<IEnumerable<SubjectEntity>> GetAllAsync();
        SubjectEntity GetSubject(string name);
        void RemoveSubject(string name);
    }
}