using ScreenOverlayManager.ViewModel;
using System.Windows;

namespace ScreenOverlayManager.View
{
    /// <summary>
    /// Interaction logic for SettingsEditorView.xaml
    /// </summary>
    public partial class SettingsEditorView : Window
    {
        public SettingsEditorViewModel ViewModel
        {
            get
            {
                if (DataContext is SettingsEditorViewModel)
                    return (SettingsEditorViewModel)DataContext;
                else
                    return null;
            }
            set
            {
                DataContext = value;
            }
        }

        public SettingsEditorView()
        {
            InitializeComponent();

            ViewModel = new SettingsEditorViewModel();
            ViewModel.PropertyChanged += (s, e) => Properties.Settings.Default.Save();
        }
    }
}
