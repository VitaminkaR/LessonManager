using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonManager.Model
{
    enum ActivityType
    {
        Lab,
        Prac,
        Lec
    }

    enum ActivityStateType
    {
        None,
        Ready,
        Passed,
        Visited,
        NotVisited,
        Answer
    }

    internal class Activity
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public ActivityType Type { get; set; }
        public DateTime ActivityTime { get; set; }
        public ActivityStateType State { get; set; }

        public Activity(string? name, ActivityType type, DateTime activityTime, ActivityStateType state)
        {
            Name = name;
            Type = type;
            ActivityTime = activityTime;
            State = state;
        }
    }
}
