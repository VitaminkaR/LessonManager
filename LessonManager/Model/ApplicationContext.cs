using Microsoft.EntityFrameworkCore;

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

    public void AddSubject(string? name, string? exam, DateTime dateTime)
    {
        if (exam == null || name == null) return;

        Subjects.Add(
            new Subject(name, (ExamType)Enum.Parse(typeof(ExamType), exam), dateTime, ExamMarkType.None)
        );
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
