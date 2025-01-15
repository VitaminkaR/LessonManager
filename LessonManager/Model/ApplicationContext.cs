using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Xml.Linq;

namespace LessonManager.Model;

internal class ApplicationContext : DbContext
{
    public ISubjectDB SubjectDB { get; set; }

    public string DbPath { get; }

    public ApplicationContext()
    {
        DbPath = System.IO.Path.Join("", "LessonManager.db");

        SubjectDB = new SubjectDB();
    }

    public void DBLoad()
    {
        SubjectDB.SubjectDBInit(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
