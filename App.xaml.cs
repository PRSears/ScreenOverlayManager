using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace ScreenOverlayManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            

            notifyIcon = (TaskbarIcon)FindResource("TrayIcon");
        }
    }
}
