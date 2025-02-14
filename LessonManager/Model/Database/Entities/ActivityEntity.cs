using CommunityToolkit.Mvvm.ComponentModel;
using LessonManager.Core.Enums;
using LessonManager.Model.Database.Entities;

namespace LessonManager.Model.Database.Entities
{
    internal partial class ActivityEntity : ObservableObject
    {
        public int ID { get; set; }
        [ObservableProperty]
        private string? m_Name;
        public SubjectEntity Subject { get; set; }
        [ObservableProperty]
        public ActivityType m_Type;
        [ObservableProperty]
        public DateTime m_ActivityTime;
        [ObservableProperty]
        public ActivityStateType m_State;

        public ActivityEntity() { }

        public ActivityEntity(string? name, SubjectEntity subject, ActivityType type, DateTime activityTime, ActivityStateType state = ActivityStateType.None)
        {
            Name = name;
            Type = type;
            Subject = subject;
            ActivityTime = activityTime;
            State = state;
        }
    }
}
