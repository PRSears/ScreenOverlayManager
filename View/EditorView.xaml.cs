using ScreenOverlayManager.Model;
using ScreenOverlayManager.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ScreenOverlayManager.View
{
    /// <summary>
    /// Interaction logic for NewOverlayView.xaml
    /// </summary>
    public partial class EditorView : Window
    {
        private EditorViewModel ViewModel
        {
            get
            {
                if (DataContext is EditorViewModel)
                    return (EditorViewModel)DataContext;
                else
                    return null;
            }
            set
            {
                DataContext = value;
            }
        }

        public EditorView()
        {
            ViewModel = new EditorViewModel();
            Init();
        }

        public EditorView(Overlay existingOverlay)
        {
            ViewModel = new EditorViewModel(existingOverlay);
            Init();
        }

        protected void Init()
        {
            InitializeComponent();
            ViewModel.RegisterCloseAction(this.Close);

            ViewModel.EditingOverlay.PropertyChanged += EditingOverlay_PropertyChanged;
        }

        private void EditingOverlay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // This shouldn't be neccessary, but the text boxes weren't updating even though
            // the binding mode is TwoWay and UpdateSourceTrigger is set to PropertyChanged.
            // Clearly PropertyChanged for X and Y is getting fired... Why the fuck aren't the
            // textboxes noticing?

            if (e.PropertyName.Equals("X"))
                BindingOperations.GetBindingExpression(xCoordBox, TextBox.TextProperty).UpdateTarget();
            else if (e.PropertyName.Equals("Y"))
                BindingOperations.GetBindingExpression(yCoordBox, TextBox.TextProperty).UpdateTarget();
            else if(e.PropertyName.Equals("Draggable"))
            {
                BindingOperations.GetBindingExpression(xCoordBox, TextBox.IsEnabledProperty).UpdateTarget();
                BindingOperations.GetBindingExpression(yCoordBox, TextBox.IsEnabledProperty).UpdateTarget();
            }
        }
    }
}
