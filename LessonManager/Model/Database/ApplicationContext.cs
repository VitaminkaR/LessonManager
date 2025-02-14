using LessonManager.Model.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace LessonManager.Model.Database;

internal class ApplicationContext : DbContext
{
    public DbSet<SubjectEntity> Subjects { get; set; } = null!;
    public DbSet<ActivityEntity> Activities { get; set; } = null!;

    public string DbPath { get; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
        DbPath = System.IO.Path.Join("", "LessonManager.db");
    }

    // очищает базу данных
    public void ClearDB()
    {
        Subjects.RemoveRange(Subjects);
        Activities.RemoveRange(Activities);

        SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
