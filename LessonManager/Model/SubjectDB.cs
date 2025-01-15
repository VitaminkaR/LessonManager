using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public void AddSubject(string name, int sn, string exam, DateTime dateTime)
        {
            if (m_Subjects.Any(s => s.Name == name && s.SemesterNumber == sn))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            m_Subjects.Add(
                new Subject(name, sn, (ExamType)Enum.Parse(typeof(ExamType), exam), dateTime, ExamMarkType.None)
            );
            m_DbContext.SaveChanges();
        }

        public void RemoveSubject(string name, int sn)
        {
            m_Subjects.Remove(
                GetSubject(name, sn)
            );
            m_DbContext.SaveChanges();
        }

        public void EditSubject(string lastname, int lsn, string name, int sn, string exam, DateTime dateTime)
        {
            if (m_Subjects.Any(s => s.Name == name && s.SemesterNumber == sn))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            Subject s = GetSubject(lastname, lsn);
            s.Name = name;
            s.SemesterNumber = sn;
            s.Exam = (ExamType)Enum.Parse(typeof(ExamType), exam);
            s.ExamDate = dateTime;
            m_DbContext.SaveChanges();
        }

        public void RemoveSemester(int sn)
        {
            m_Subjects.RemoveRange(m_Subjects.Where(s => s.SemesterNumber == sn).Select(s => s).ToArray());
            m_DbContext.SaveChanges();
        }

        public Subject GetSubject(string name, int sn)
        {
            return m_Subjects.Where(s => s.Name == name && s.SemesterNumber == sn).Select(s => s).First();
        }
    }
}
