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
        }
    }
}
