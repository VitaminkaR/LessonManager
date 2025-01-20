using Microsoft.EntityFrameworkCore;
using System;

namespace LessonManager.Model;

internal class ApplicationContext : DbContext
{
    public DbSet<Subject> Subjects { get; set; } = null!;
    public ISubjectDB SubjectDB { get; set; }
    public DbSet<Activity> Activities { get; set; } = null!;
    public IActivityDB ActivityDB { get; set; }

    public string DbPath { get; }

    public ApplicationContext()
    {
        DbPath = System.IO.Path.Join("", "LessonManager.db");

        SubjectDB = new SubjectDB();
        ActivityDB = new ActivityDB();
    }

    public void DBLoad()
    {
        SubjectDB.SubjectDBInit(Subjects, this);
        ActivityDB.ActivityDBInit(Activities, this);
    }

    // очищает базу данных
    public void ClearDB()
    {
        Subjects.RemoveRange(Subjects);
        Activities.RemoveRange(Activities);

        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
