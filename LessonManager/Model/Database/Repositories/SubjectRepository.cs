using LessonManager.Core.Enums;
using LessonManager.Model.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.Model.Database.Repositories
{
    internal class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationContext m_DbContext;

        public SubjectRepository(ApplicationContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public async Task AddAsync(string name, string exam, DateTime dateTime)
        {
            await m_DbContext.Subjects.AddAsync(
                new SubjectEntity(name, (ExamType)Enum.Parse(typeof(ExamType), exam), dateTime, ExamMarkType.None)
            );
            await m_DbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(string name)
        {
            SubjectEntity entity = await GetAsync(name);
            m_DbContext.Subjects.Remove(entity);
            await m_DbContext.SaveChangesAsync();
        }

        public async Task EditAsync(string lastname, string name, string exam, DateTime dateTime)
        {
            SubjectEntity s = await GetAsync(lastname);
            s.Name = name;
            s.Exam = (ExamType)Enum.Parse(typeof(ExamType), exam);
            s.ExamDate = dateTime;
            m_DbContext.Update(s);
            await m_DbContext.SaveChangesAsync();
        }

        public async Task AddAsync(SubjectEntity subject)
        {
            await m_DbContext.Subjects.AddAsync(subject);
            await m_DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubjectEntity>> GetAsync() =>
            await m_DbContext.Subjects.AsNoTracking().ToArrayAsync();

        public async Task<SubjectEntity> GetAsync(string name) =>
            await m_DbContext.Subjects.AsNoTracking().Where(s => s.Name == name).Select(s => s).FirstAsync();
    }
}
