using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace LessonManager.Model;

internal class ApplicationContext : DbContext
{
    public DbSet<Subject> Subjects { get; set; } = null!;

    public string DbPath { get; }

    public ApplicationContext()
    {
        DbPath = System.IO.Path.Join("", "LessonManager.db");
    }

    public void DBLoad()
    {
        Subjects.Load();
    }

    public void AddSubject(string? name, int sn, string? exam, DateTime dateTime)
    {
        if (exam == null || name == null)
        {
            MessageBox.Show("Не все поля заполненные");
            return;
        }

        if (Subjects.Any(s => s.Name == name && s.SemesterNumber == sn))
        {
            MessageBox.Show("Такой предмет в этом семестре уже существует");
            return;
        }

        Subjects.Add(
            new Subject(name, sn, (ExamType)Enum.Parse(typeof(ExamType), exam), dateTime, ExamMarkType.None)
        );
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
