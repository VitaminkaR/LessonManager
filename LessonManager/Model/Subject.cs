using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
