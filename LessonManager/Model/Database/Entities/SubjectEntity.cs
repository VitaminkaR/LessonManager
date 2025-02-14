using CommunityToolkit.Mvvm.ComponentModel;
using LessonManager.Core.Enums;

namespace LessonManager.Model.Database.Entities
{
    internal class SubjectEntity : ObservableObject
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public ExamType Exam { get; set; }
        public DateTime ExamDate { get; set; }
        public ExamMarkType ExamMark { get; set; } = ExamMarkType.None;

        public ICollection<ActivityEntity> Activities { get; set; }

        public SubjectEntity() { }

        public SubjectEntity(string? name, ExamType exam, DateTime dateTime, ExamMarkType examMark)
        {
            Name = name;
            Exam = exam;
            ExamDate = dateTime;
            ExamMark = examMark;
        }
    }
}
