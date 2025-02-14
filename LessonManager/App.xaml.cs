using LessonManager.Model;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace LessonManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public readonly IConfiguration Configuration;

        static App()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.json")
                .Build();
        }

        static internal ApplicationContext ApplicationContext { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.ApplicationContext = new ApplicationContext();
        }
    }

}
