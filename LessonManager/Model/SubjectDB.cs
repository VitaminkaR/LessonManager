using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.Model
{
    internal class SubjectDB : ISubjectDB
    {
        private DbContext m_DbContext;
        private DbSet<Subject> m_Subjects;

        public void SubjectDBInit(DbSet<Subject> dbset, DbContext context)
        {
            m_DbContext = context;
            m_Subjects = dbset;
            m_Subjects.Load();
        }

        public void AddSubject(string name, string exam, DateTime dateTime)
        {
            if (m_Subjects.Any(s => s.Name == name))
            {
                MessageBox.Show("Такой предмет уже существует");
                return;
            }

            m_Subjects.Add(
                new Subject(name, (ExamType)Enum.Parse(typeof(ExamType), exam), dateTime, ExamMarkType.None)
            );
            m_DbContext.SaveChanges();
        }

        public void RemoveSubject(string name)
        {
            m_Subjects.Remove(
                GetSubject(name)
            );
            m_DbContext.SaveChanges();
        }

        public void EditSubject(string lastname, string name, string exam, DateTime dateTime)
        {
            if (m_Subjects.Any(s => s.Name == name))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            Subject s = GetSubject(lastname);
            s.Name = name;
            s.Exam = (ExamType)Enum.Parse(typeof(ExamType), exam);
            s.ExamDate = dateTime;
            m_DbContext.SaveChanges();
        }

        public Subject GetSubject(string name)
        {
            return m_Subjects.Where(s => s.Name == name).Select(s => s).First();
        }

        public void AddSubject(Subject subject)
        {
            if (m_Subjects.Any(s => s.Name == subject.Name))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            m_Subjects.Add(subject);
            m_DbContext.SaveChanges();
        }
    }
}
