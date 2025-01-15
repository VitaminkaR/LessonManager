using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonManager.Model
{
    internal interface IActivityDB
    {
        public void ActivityDBInit(DbSet<Activity> dbset, DbContext context);

        public void AddActivity(string? name, Subject subject, ActivityType type, DateTime activityTime);

        public void RemoveActivity(string name, Subject subject);

        public void EditActivity(string predname, Subject predsubject, string? name, ActivityType type, DateTime activityTime, ActivityStateType state);

        public Activity GetActivity(string name, Subject subject);
    }
}
