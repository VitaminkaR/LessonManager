using LessonManager.Model.Database;
using LessonManager.Model.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LessonManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public App CurrentApplication = null!;

        public readonly IConfiguration Configuration;

        public readonly IServiceCollection services;

        public App()
        {
            CurrentApplication = this;

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.json")
                .Build();

            services = new ServiceCollection();
            services.AddSingleton<ApplicationContext>(new ApplicationContext(
                new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite($"Data Source={Configuration["DBPATH"]}")
                    .EnableSensitiveDataLogging()
                    .Options
                ));
            services.AddSingleton<IActivityRepository, ActivityRepository>();
            services.AddSingleton<ISubjectRepository, SubjectRepository>();
        }
    }
}
