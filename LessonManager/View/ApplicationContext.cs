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
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
