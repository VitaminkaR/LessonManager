using CommunityToolkit.Mvvm.ComponentModel;

namespace LessonManager.Model
{
    internal partial class Activity : ObservableObject
    {
        public int ID { get; set; }
        [ObservableProperty]
        private string? m_Name;
        public Subject Subject { get; set; }
        [ObservableProperty]
        public ActivityType m_Type;
        [ObservableProperty]
        public DateTime m_ActivityTime;
        [ObservableProperty]
        public ActivityStateType m_State;

        public Activity() { }

        public Activity(string? name, Subject subject, ActivityType type, DateTime activityTime, ActivityStateType state = ActivityStateType.None)
        {
            Name = name;
            Type = type;
            Subject = subject;
            ActivityTime = activityTime;
            State = state;
        }
    }
}
