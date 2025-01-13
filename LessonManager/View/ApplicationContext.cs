using LessonManager.Model;
using Microsoft.EntityFrameworkCore;

namespace LessonManager.View
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }

        public string DbPath { get; }

        public ApplicationContext()
        {
            DbPath = System.IO.Path.Join("", "LessonManager.db");
        }

        public void DBLoad()
        {
            Subjects.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
