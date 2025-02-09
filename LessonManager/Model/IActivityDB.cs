using Microsoft.EntityFrameworkCore;

namespace LessonManager.Model
{
    internal interface IActivityDB
    {
        public void ActivityDBInit(DbSet<Activity> dbset, DbContext context);

        public void AddActivity(string? name, Subject subject, ActivityType type, DateTime activityTime);

        public void AddActivity(Activity activity);

        public void RemoveActivity(string name, Subject subject);

        public void RemoveActivity(Activity activity);

        public void EditActivity(string predname, Subject predsubject, string? name, ActivityType type, DateTime activityTime, ActivityStateType state);

        public void EditActivity(Activity activity);

        public Activity GetActivity(string name, Subject subject);

        public ICollection<Activity> GetAllActivitiesOfTypeFromSubject(Subject subject, ActivityType type);
    }
}
