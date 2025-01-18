using CommunityToolkit.Mvvm.ComponentModel;

namespace LessonManager.Model
{
    enum ExamType
    {
        Exam,
        Test,
        DifTest
    }

    enum ExamMarkType
    {
        None,
        Great,
        Well,
        Passed,
        NotPassed
    }

    internal class Subject : ObservableObject
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int SemesterNumber { get; set; }
        public ExamType Exam { get; set; }
        public DateTime ExamDate { get; set; }
        public ExamMarkType ExamMark { get; set; } = ExamMarkType.None;

        public ICollection<Activity> Activities { get; set; }

        public Subject() { }

        public Subject(string? name, int semesterNumber, ExamType exam, DateTime dateTime, ExamMarkType examMark)
        {
            Name = name;
            SemesterNumber = semesterNumber;
            Exam = exam;
            ExamDate = dateTime;
            ExamMark = examMark;
        }
    }
}
