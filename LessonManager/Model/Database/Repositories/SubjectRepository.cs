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

        public void AddSubject(string name, string exam, DateTime dateTime)
        {
            if (m_DbContext.Subjects.Any(s => s.Name == name))
            {
                MessageBox.Show("Такой предмет уже существует");
                return;
            }

            m_DbContext.Subjects.Add(
                new SubjectEntity(name, (ExamType)Enum.Parse(typeof(ExamType), exam), dateTime, ExamMarkType.None)
            );
            m_DbContext.SaveChanges();
        }

        public void RemoveSubject(string name)
        {
            m_DbContext.Subjects.Remove(
                GetSubject(name)
            );
            m_DbContext.SaveChanges();
        }

        public void EditSubject(string lastname, string name, string exam, DateTime dateTime)
        {
            if (m_DbContext.Subjects.Any(s => s.Name == name))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            SubjectEntity s = GetSubject(lastname);
            s.Name = name;
            s.Exam = (ExamType)Enum.Parse(typeof(ExamType), exam);
            s.ExamDate = dateTime;
            m_DbContext.SaveChanges();
        }

        public SubjectEntity GetSubject(string name)
        {
            return m_DbContext.Subjects.Where(s => s.Name == name).Select(s => s).First();
        }

        public void AddSubject(SubjectEntity subject)
        {
            if (m_DbContext.Subjects.Any(s => s.Name == subject.Name))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            m_DbContext.Subjects.Add(subject);
            m_DbContext.SaveChanges();
        }
    }
}
