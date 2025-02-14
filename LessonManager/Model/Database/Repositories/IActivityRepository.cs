using LessonManager.Core.Enums;
using LessonManager.Model.Database.Entities;

namespace LessonManager.Model.Database.Repositories
{
    internal interface IActivityRepository
    {
        void AddActivity(ActivityEntity activity);
        void AddActivity(string? name, SubjectEntity subject, ActivityType type, DateTime activityTime);
        void EditActivity(ActivityEntity activity);
        void EditActivity(string predname, SubjectEntity predsubject, string? name, ActivityType type, DateTime activityTime, ActivityStateType state);
        ActivityEntity GetActivity(string name, SubjectEntity subject);
        ICollection<ActivityEntity> GetAllActivitiesOfTypeFromSubject(SubjectEntity subject, ActivityType type);
        void RemoveActivity(ActivityEntity activity);
        void RemoveActivity(string name, SubjectEntity subject);
    }
}