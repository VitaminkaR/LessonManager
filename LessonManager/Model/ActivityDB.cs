using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.Model
{
    internal class ActivityDB : IActivityDB
    {
        private DbContext? m_DbContext = null!;
        private DbSet<Activity> m_Activity;

        public void ActivityDBInit(DbSet<Activity> dbset, DbContext context)
        {
            m_DbContext = context;
            m_Activity = dbset;
            m_Activity.Load();
        }

        public void AddActivity(string? name, Subject subject, ActivityType type, DateTime activityTime)
        {
            m_Activity.Add(
                new Activity(name, subject, type, activityTime)
                );
            m_DbContext.SaveChanges();
        }

        public void AddActivity(Activity activity)
        {
            m_Activity.Add(activity);
            try
            {
                m_DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {activity.Name}");
            }
        }

        public void EditActivity(string predname, Subject predsubject, string? name, ActivityType type, DateTime activityTime, ActivityStateType state)
        {
            Activity a = GetActivity(predname, predsubject);
            a.Name = name;
            a.Type = type;
            a.ActivityTime = activityTime;
            a.State = state;
            m_DbContext.SaveChanges();
        }

        public void EditActivity(Activity activity)
        {
            m_Activity.Update(activity);
            m_DbContext.SaveChanges();
        }

        public Activity GetActivity(string name, Subject subject)
        {
            return m_Activity.Where(s => s.Name == name && s.Subject == subject).Select(s => s).First();
        }

        public ICollection<Activity> GetAllActivitiesOfTypeFromSubject(Subject subject, ActivityType type)
        {
            return m_Activity.Where(s => s.Subject == subject && s.Type == type).Select(s => s).ToArray();
        }

        public void RemoveActivity(string name, Subject subject)
        {
            m_Activity.Remove(
                GetActivity(name, subject)
            );
            m_DbContext.SaveChanges();
        }
    }
}
