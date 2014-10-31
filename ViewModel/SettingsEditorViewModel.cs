using ScreenOverlayManager.Properties;

namespace ScreenOverlayManager.ViewModel
{
    public class SettingsEditorViewModel : Extender.WPF.ViewModel
    {
        #region //Default settings aliases
        public int AutosaveTimer
        {
            get
            {
                return Settings.Default.AutosaveTimer;
            }
            set
            {
                Settings.Default.AutosaveTimer = value;
                OnPropertyChanged("AutosaveTimer");
            }
        }

        public bool Debugging
        {
            get
            {
                return Settings.Default.Debugging;
            }
            set
            {
                Settings.Default.Debugging = value;
                OnPropertyChanged("Debugging");
                OnPropertyChanged("DebuggingOptionsVisibility");
            }
        }

        public string LogfilePath
        {
            get
            {
                return Settings.Default.LogfilePath;
            }
            set
            {
                Settings.Default.LogfilePath = value;
                OnPropertyChanged("LogfilePath");
            }
        }

        public System.Windows.Visibility DebuggingOptionsVisibility
        {
            get
            {
                return Debugging ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
        }

        public System.Windows.Media.Color DefaultColor1
        {
            get
            {
                return Settings.Default.DefaultOverlayColor1;
            }
            set
            {
                Settings.Default.DefaultOverlayColor1 = value;
                OnPropertyChanged("DefaultColor1");
            }
        }

        public System.Windows.Media.Color DefaultColor2
        {
            get
            {
                return Settings.Default.DefaultOverlayColor2;
            }
            set
            {
                Settings.Default.DefaultOverlayColor2 = value;
                OnPropertyChanged("DefaultColor2");
            }
        }

        public bool DrawBorder
        {
            get
            {
                return Settings.Default.DefaultOverlayDrawBorder;
            }
            set
            {
                Settings.Default.DefaultOverlayDrawBorder = value;
                OnPropertyChanged("DrawBorder");
            }
        }

        public bool DrawCrosshair
        {
            get
            {
                return Settings.Default.DefaultOverlayDrawCrosshair;
            }
            set
            {
                Settings.Default.DefaultOverlayDrawCrosshair = value;
                OnPropertyChanged("DrawBorder");
            }
        }

        public string FilenameFormat
        {
            get
            {
                return Settings.Default.DefaultOverlayFilenameFormat;
            }
            set
            {
                Settings.Default.DefaultOverlayFilenameFormat = value;
                OnPropertyChanged("FilenameFormat");
            }
        }

        public double OverlayHeight
        {
            get
            {
                return Settings.Default.DefaultOverlayHeight;
            }
            set
            {
                Settings.Default.DefaultOverlayHeight = value;
                OnPropertyChanged("OverlayHeight");
            }
        }

        public double OverlayPosition_X
        {
            get
            {
                return Settings.Default.DefaultOverlayPosition.X;
            }
            set
            {
                Settings.Default.DefaultOverlayPosition = new System.Windows.Point
                    (
                        value,
                        Settings.Default.DefaultOverlayPosition.Y
                    );
                OnPropertyChanged("OverlayPosition_X");
            }
        }

        public double OverlayPosition_Y
        {
            get
            {
                return Settings.Default.DefaultOverlayPosition.Y;
            }
            set
            {
                Settings.Default.DefaultOverlayPosition = new System.Windows.Point
                    (
                        Settings.Default.DefaultOverlayPosition.X,
                        value
                    );
                OnPropertyChanged("OverlayPosition_Y");
            }
        }

        public double StrokeThickness
        {
            get
            {
                return Settings.Default.DefaultOverlayStrokeThickness;
            }
            set
            {
                Settings.Default.DefaultOverlayStrokeThickness = value;
                OnPropertyChanged("StrokeThickness");
            }
        }

        public double OverlayWidth
        {
            get
            {
                return Settings.Default.DefaultOverlayWidth;
            }
            set
            {
                Settings.Default.DefaultOverlayWidth = value;
                OnPropertyChanged("Width");
            }
        }

        public string SaveDirectory
        {
            get
            {
                return Settings.Default.SavedDirectory;
            }
            set
            {
                Settings.Default.SavedDirectory = value;
                OnPropertyChanged("SaveDirectory");
            }
        }

        public bool StartMinimizedToTray
        {
            get
            {
                return Settings.Default.StartMinimizedToTray;
            }
            set
            {
                Settings.Default.StartMinimizedToTray = value;
                OnPropertyChanged("StartMinimizedToTray");
            }
        }

        public int UpdateInterval
        {
            get
            {
                return Settings.Default.UpdateInterval;
            }
            set
            {
                Settings.Default.UpdateInterval = value;
                OnPropertyChanged("UpdateInterval");
            }
        }
        #endregion

        #region //ICommands



        #endregion

        public SettingsEditorViewModel()
        {

        }
    }
}
