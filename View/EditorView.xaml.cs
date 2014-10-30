using ScreenOverlayManager.Model;
using ScreenOverlayManager.ViewModel;
using System.Windows;
using System.Windows.Data;
using ColorCanvas = Xceed.Wpf.Toolkit.ColorCanvas;

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

        private Binding PColorCanvasBinding;
        private Binding SColorCanvasBinding;

        protected void Init()
        {
            InitializeComponent();
            ViewModel.RegisterCloseAction(this.Close);

            //
            // Suspend bindings (bindings are restored in OnContentRendered. See comment there for details.)
            PColorCanvasBinding = BindingOperations.GetBinding(PrimaryColorCanvas, ColorCanvas.SelectedColorProperty);
            SColorCanvasBinding = BindingOperations.GetBinding(SecondaryColorCanvas, ColorCanvas.SelectedColorProperty);

            BindingOperations.ClearBinding(PrimaryColorCanvas, ColorCanvas.SelectedColorProperty);
            BindingOperations.ClearBinding(SecondaryColorCanvas, ColorCanvas.SelectedColorProperty);
        }

        protected override void OnContentRendered(System.EventArgs e)
        {
            base.OnContentRendered(e);

            // There was/is a bug (I think) in the ColorCanvas where color channels set to 0 in the bound object were 
            // sometimes changed to 4 after Window.Show() is called.
            // Removing the binding until after the window is shown is my shitty work around.
            PrimaryColorCanvas.SelectedColor = ViewModel.EditingOverlay.PrimaryColor;
            SecondaryColorCanvas.SelectedColor = ViewModel.EditingOverlay.SecondaryColor;

            PrimaryColorCanvas.SetBinding(ColorCanvas.SelectedColorProperty, PColorCanvasBinding);
            SecondaryColorCanvas.SetBinding(ColorCanvas.SelectedColorProperty, SColorCanvasBinding);
        }

        protected override void OnKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == System.Windows.Input.Key.Return)
                ViewModel.FinishedEditingCommand.Execute(null);
        }
    }
}
