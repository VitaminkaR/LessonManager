using LessonManager.View;
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
        static internal ApplicationContext? ApplicationContext { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.ApplicationContext = new ApplicationContext();
        }
    }

}
