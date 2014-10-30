using Extender;
using Extender.WPF;
using ScreenOverlayManager.Model;
using System.Windows.Input;
using FontWeights = System.Windows.FontWeights;

namespace ScreenOverlayManager.ViewModel
{
    public class EditorViewModel : Extender.WPF.ViewModel
    {
        public ICommand CancelCommand           { get; private set; }
        public ICommand ResetChangesCommand     { get; private set; }
        public ICommand FinishedEditingCommand  { get; private set; }
        public ICommand ToggleDragCommand       { get; private set; }


        public Overlay EditingOverlay
        {
            get
            {
                return _EditingOverlay;
            }
            set
            {
                _EditingOverlay = value;
                OnPropertyChanged("EditingOverlay");
            }
        }

        private Overlay _EditingOverlay;

        private Overlay InitialState { get; set; }

        public EditorViewModel(Overlay overlayToEdit) 
        {
            this._EditingOverlay = overlayToEdit;
            this.InitialState = this.EditingOverlay.Copy();

            this.CancelCommand          = new RelayCommand(() => Cancel());
            this.ResetChangesCommand    = new RelayCommand(() => Reset(true));
            this.FinishedEditingCommand = new RelayCommand(() => CloseCommand.Execute(null));
            this.ToggleDragCommand      = new RelayCommand
            (
                () => EditingOverlay.Draggable = !EditingOverlay.Draggable,
                () =>
                {
                    return (EditingOverlay != null) && EditingOverlay.IsVisible;
                }
            );

            this.EditingOverlay.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("Draggable"))
                {
                    // Update 'Quick Positioning' button text
                    this.OnPropertyChanged("ToggleDragButtonText");
                    this.OnPropertyChanged("ToggleDragButtonTextWeight");
                }
            };
        }

        // TODO Debug why the fuck the blue channel changes to 4 if it's set to 0 upon load

        public EditorViewModel() : this(new Overlay()) 
        {
            this._EditingOverlay.LoadDefaults();
        }
        
        public string ToggleDragButtonText
        {
            get
            {
                if (EditingOverlay == null) return "Start Quick Positioning";

                return EditingOverlay.Draggable ? "Stop Quick Positioning" : "Start Quick Positioning";
            }
        }

        public System.Windows.FontWeight ToggleDragButtonTextWeight
        {
            get
            {
                return EditingOverlay.Draggable ? FontWeights.Bold : FontWeights.Normal;
            }
        }

        public bool IsCoordBoxEnabled
        {
            get
            {
                return !EditingOverlay.Draggable;
            }
        }

        /// <summary>
        /// Reverts any changes made to EditingOverlay back to the state it was in 
        /// on initialization of the editor window.
        /// </summary>
        /// <param name="confirmBeforeReset">
        /// When true a confirmation dialog must be accepted before the data is reset.
        /// </param>
        /// <returns>True if the data is reset.</returns>
        protected bool Reset(bool confirmBeforeReset)
        {
            bool confirmed = false;

            if (confirmBeforeReset)
            {
                confirmed = ConfirmationDialog.Show
                (
                    "Discard changes?",
                    "Are you sure you want to discard the changes you just made?"
                );
            }

            if (confirmed || !confirmBeforeReset)
            {
                EditingOverlay.CopyFrom(InitialState);
                return true;
            }
            else return false;
        }

        protected void Cancel()
        {
            if (Reset(true))
                CloseCommand.Execute(null);
        }

        private bool DEBUG
        {
            get
            {
                return Properties.Settings.Default.Debugging;
            }
        }
    }
}
