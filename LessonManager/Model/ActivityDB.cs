using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.Model
{
    internal class ActivityDB : IActivityDB
    {
        private DbContext? m_DbContext = null!;

        public DbSet<Activity> Activities { get; set; } = null!;

        public void ActivityDBInit(DbContext context)
        {
            m_DbContext = context;
            Activities.Load();
        }

        public void AddActivity(string? name, Subject subject, ActivityType type, DateTime activityTime)
        {
            Activities.Add(
                new Activity(name, subject, type, activityTime)
                );
        }

        public void EditActivity(string predname, Subject predsubject, string? name, ActivityType type, DateTime activityTime, ActivityStateType state)
        {
            if (Activities.Any(s => s.Name == name && s.Subject == predsubject))
            {
                MessageBox.Show("Такой предмет в этом семестре уже существует");
                return;
            }

            Activity a = GetActivity(predname, predsubject);
            a.Name = name;
            a.Type = type;
            a.ActivityTime = activityTime;
            a.State = state;
            m_DbContext.SaveChanges();
        }

        public Activity GetActivity(string name, Subject subject)
        {
            return Activities.Where(s => s.Name == name && s.Subject == subject).Select(s => s).First();
        }

        public void RemoveActivity(string name, Subject subject)
        {
            Activities.Remove(
                GetActivity(name, subject)
            );
            m_DbContext.SaveChanges();
        }
    }
}
