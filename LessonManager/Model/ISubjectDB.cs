using Microsoft.EntityFrameworkCore;

namespace LessonManager.Model
{
    internal interface ISubjectDB
    {
        public void SubjectDBInit(DbSet<Subject> dbset, DbContext context);

        public void AddSubject(string name, string exam, DateTime dateTime);

        public void AddSubject(Subject subject);

        public void RemoveSubject(string name);

        public void EditSubject(string lastname, string name, string exam, DateTime dateTime);

        public Subject GetSubject(string name);
    }
}
