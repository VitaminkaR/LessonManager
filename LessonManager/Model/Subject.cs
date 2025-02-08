using CommunityToolkit.Mvvm.ComponentModel;

namespace LessonManager.Model
{
    internal class Subject : ObservableObject
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public ExamType Exam { get; set; }
        public DateTime ExamDate { get; set; }
        public ExamMarkType ExamMark { get; set; } = ExamMarkType.None;

        public ICollection<Activity> Activities { get; set; }

        public Subject() { }

        public Subject(string? name, ExamType exam, DateTime dateTime, ExamMarkType examMark)
        {
            Name = name;
            Exam = exam;
            ExamDate = dateTime;
            ExamMark = examMark;
        }
    }
}
