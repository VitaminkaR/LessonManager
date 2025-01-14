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
        public ExamType Exam { get; set; }
        public DateTime DateTime { get; set; }
        public ExamMarkType ExamMark { get; set; } = ExamMarkType.None;

        public Subject() { }

        public Subject(string? name, ExamType exam, DateTime dateTime, ExamMarkType examMark)
        {
            Name = name;
            Exam = exam;
            DateTime = dateTime;
            ExamMark = examMark;
        }
    }
}
