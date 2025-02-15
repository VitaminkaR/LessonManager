using LessonManager.Model.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace LessonManager.Model.Database;

internal class ApplicationContext : DbContext
{
    public DbSet<SubjectEntity> Subjects { get; set; } = null!;
    public DbSet<ActivityEntity> Activities { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
        Database.EnsureCreated();
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
