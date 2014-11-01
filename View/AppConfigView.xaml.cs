using Hardcodet.Wpf.TaskbarNotification;
using ScreenOverlayManager.ViewModel;
using System.Windows;

namespace ScreenOverlayManager.View
{
    /// <summary>
    /// Interaction logic for AppConfigView.xaml
    /// </summary>
    public partial class AppConfigView : Window
    {
        public AppConfigViewModel ViewModel
        {
            get
            {
                if (DataContext is AppConfigViewModel)
                    return (AppConfigViewModel)DataContext;
                else
                    return null;
            }
            set
            {
                DataContext = value;
            }
        }
        private TaskbarIcon Tray;

        public AppConfigView()
        {
            InitializeComponent();

            this.ViewModel = new AppConfigViewModel();
            this.ViewModel.RegisterCloseAction(() => this.Close());
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // prevents listings getting highlighted (such as on right click)
            // so that checking the radio button is the only visual queue
            this.OverlaysListBox.SelectionChanged += (s, e) => OverlaysListBox.UnselectAll();

            Tray = (TaskbarIcon)this.TryFindResource("TrayIcon");            
            if(Tray != null) this.Tray.DataContext = this.ViewModel;

            UpdateWindowState();
        }

        protected void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Overlays"))
                this.OverlaysListBox.Items.Refresh();
            else if (e.PropertyName.Equals("ConfiguratorVisible") || e.PropertyName.Equals("ConfiguratorShowInTaskbar"))
                this.UpdateWindowState();
            else if (e.PropertyName.Equals("RequestingFocus"))
                this.CheckFocus();
        }

        public void UpdateWindowState()
        {
            if (ViewModel == null) return;

            this.Visibility     = ViewModel.ConfiguratorVisible ? Visibility.Visible : Visibility.Hidden;
            this.ShowInTaskbar  = ViewModel.ConfiguratorShowInTaskbar;
        }

        public void CheckFocus()
        {
            if(ViewModel.RequestingFocus)
            {
                this.Activate();
                ViewModel.RequestingFocus = false;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!ViewModel.AppIsClosing)
            {
                e.Cancel = true;
                ViewModel.HideManagerCommand.Execute(null);
                base.OnClosing(e);
            }
            else
            {
                base.OnClosing(e);
                Application.Current.Shutdown();
            }
        }
    }
}
