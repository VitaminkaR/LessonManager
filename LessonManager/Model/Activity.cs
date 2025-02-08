namespace LessonManager.Model
{
    internal class Activity
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public Subject Subject { get; set; }
        public ActivityType Type { get; set; }
        public DateTime ActivityTime { get; set; }
        public ActivityStateType State { get; set; }

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
