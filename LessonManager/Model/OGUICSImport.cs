using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.IO;
using System.Xml.Linq;
namespace LessonManager.Model
{
    internal class OGUICSImport : IISCImport
    {
        private Calendar m_Calendar;

        public ICollection<Subject> GetSubjects()
        {
            Dictionary<string, Subject> subjects = new Dictionary<string, Subject>();
            foreach (var ev in m_Calendar.Events)
            {
                string summary = ev.Summary;
                string subjectName = summary.Split(" (")[0];

                // создаем дисциплину если ее не было раньше
                if (!subjects.ContainsKey(subjectName))
                {
                    Subject subject = new Subject(
                    subjectName,
                    ExamType.Test,
                    DateTime.MinValue,
                    ExamMarkType.None
                    );

                    subject.Activities = new List<Activity>();
                    subjects.Add(subjectName, subject);
                }

                // определение времени
                IDateTime dateTime = ev.DtStart;
                DateTime time = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

                ActivityType type = ActivityType.Lec; // тип занятия
                string activityName = ""; // шаблонное имя занятия (лекция или лабораторная и т д)
                // устанавливаем дату экзамена
                if (summary.Contains("(конс)"))
                    continue;
                // устанавливаем дату и тип промежуточной аттестации
                else
                if (summary.Contains("(экз)"))
                {
                    subjects[subjectName].ExamDate = time;
                    subjects[subjectName].Exam = ExamType.Exam;
                    continue;
                }
                else
                if (summary.Contains("(зачет)"))
                {
                    subjects[subjectName].ExamDate = time;
                    subjects[subjectName].Exam = ExamType.Test;
                    continue;
                }
                // определение типа занятия
                else
                if (summary.Contains("(лаб)"))
                {
                    type = ActivityType.Lab;
                    activityName = "Лабораторная работа №";
                }
                else
                if (summary.Contains("(пр)"))
                {
                    type = ActivityType.Prac;
                    activityName = "Практическая работа №";
                }
                else
                if (summary.Contains("(лек)"))
                {
                    type = ActivityType.Lec;
                    activityName = "Лекция №";
                }

                // проверка на ту же лабу (лаба - 2 занятия)
                if (subjects[subjectName].Activities.Where(
                    a => a.Type == ActivityType.Lab &&
                    a.ActivityTime.Year == time.Year &&
                    a.ActivityTime.Month == time.Month &&
                    a.ActivityTime.Day == time.Day &&
                    a.ActivityTime.Hour < time.Hour + 3
                    ).Count() > 0)
                    continue;

                subjects[subjectName].Activities.Add(
                    new Activity(
                        $"{activityName}",
                        subjects[subjectName],
                        type,
                        time
                    )
                );
            }

            // правильная нумерация занятий
            foreach (var subject in subjects.Values)
            {
                int[] counts = { 1, 1, 1 };
                foreach (var activity in subject.Activities.OrderBy((a) => a.ActivityTime))
                {
                    if(activity.Type == ActivityType.Lec)
                    {
                        activity.Name += counts[0].ToString();
                        counts[0]++;
                        continue;
                    }
                    if (activity.Type == ActivityType.Prac)
                    {
                        activity.Name += counts[1].ToString();
                        counts[1]++;
                        continue;
                    }
                    if (activity.Type == ActivityType.Lab)
                    {
                        activity.Name += counts[2].ToString();
                        counts[2]++;
                        continue;
                    }
                }
            }

            return subjects.Values.ToList();
        }

        public void Init(string icsString)
        {
            throw new NotImplementedException();
        }

        public void Init(Stream icsStream)
        {
            m_Calendar = Ical.Net.Calendar.Load(icsStream);
        }
    }
}
