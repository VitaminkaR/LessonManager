using LessonManager.Core.Enums;
using LessonManager.Model.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.Model.Database.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private ApplicationContext m_DbContext = null!;

        public ActivityRepository(ApplicationContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public void AddActivity(string? name, SubjectEntity subject, ActivityType type, DateTime activityTime)
        {
            m_DbContext.Activities.Add(
                new ActivityEntity(name, subject, type, activityTime)
                );
            m_DbContext.SaveChanges();
        }

        public void AddActivity(ActivityEntity activity)
        {
            m_DbContext.Activities.Add(activity);
            m_DbContext.SaveChanges();
        }

        public void EditActivity(string predname, SubjectEntity predsubject, string? name, ActivityType type, DateTime activityTime, ActivityStateType state)
        {
            ActivityEntity a = GetActivity(predname, predsubject);
            a.Name = name;
            a.Type = type;
            a.ActivityTime = activityTime;
            a.State = state;
            m_DbContext.SaveChanges();
        }

        public void EditActivity(ActivityEntity activity)
        {
            m_DbContext.Activities.Update(activity);
            m_DbContext.SaveChanges();
        }

        public ActivityEntity GetActivity(string name, SubjectEntity subject)
        {
            return m_DbContext.Activities.Where(s => s.Name == name && s.Subject == subject).Select(s => s).First();
        }

        public ICollection<ActivityEntity> GetAllActivitiesOfTypeFromSubject(SubjectEntity subject, ActivityType type)
        {
            return m_DbContext.Activities.Where(s => s.Subject == subject && s.Type == type).Select(s => s).ToArray();
        }

        public void RemoveActivity(string name, SubjectEntity subject)
        {
            m_DbContext.Activities.Remove(
                GetActivity(name, subject)
            );
            m_DbContext.SaveChanges();
        }

        public void RemoveActivity(ActivityEntity activity)
        {
            m_DbContext.Activities.Remove(
                activity
            );
            m_DbContext.SaveChanges();
        }
    }
}
