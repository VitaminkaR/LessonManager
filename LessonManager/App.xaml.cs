using LessonManager.Model;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LessonManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string VERSION = "0.9";

        static internal ApplicationContext ApplicationContext { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.ApplicationContext = new ApplicationContext();
        }
    }

}
