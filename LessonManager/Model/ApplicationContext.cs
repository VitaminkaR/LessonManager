using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Xml.Linq;

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

    public void AddSubject(string name, int sn, string exam, DateTime dateTime)
    {
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

    public void RemoveSubject(string name, int sn)
    {
        Subjects.Remove(
            GetSubject(name, sn)
        );
        SaveChanges();
    }

    public void RemoveSemester(int sn)
    {
        Subjects.RemoveRange(Subjects.Where(s => s.SemesterNumber == sn).Select( s => s).ToArray());
        SaveChanges();
    }

    public Subject GetSubject(string name, int sn)
    {
        return Subjects.Where(s => s.Name == name && s.SemesterNumber == sn).Select(s => s).First();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
