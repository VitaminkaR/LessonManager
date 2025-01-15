using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonManager.Model
{
    internal interface ISubjectDB
    {
        public void SubjectDBInit(DbSet<Subject> dbset, DbContext context);

        public void AddSubject(string name, int sn, string exam, DateTime dateTime);

        public void RemoveSubject(string name, int sn);

        public void EditSubject(string lastname, int lsn, string name, int sn, string exam, DateTime dateTime);

        public void RemoveSemester(int sn);

        public Subject GetSubject(string name, int sn);
    }
}
