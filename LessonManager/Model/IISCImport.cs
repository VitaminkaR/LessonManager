using Ical.Net;
using System.IO;

namespace LessonManager.Model
{
    internal interface IISCImport
    {
        public void Init(string icsString);
        public void Init(Stream icsStream);

        public ICollection<Subject> GetSubjects();
    }
}
