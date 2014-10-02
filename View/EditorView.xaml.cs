using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using ScreenOverlayManager.ViewModel;

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
            InitializeComponent();

            this.ViewModel = new EditorViewModel();

        }

        //public EditorView(System.Collections.ObjectModel.ObservableCollection<Model.Overlay_Form> overlays)
        //{
        //    InitializeComponent();

        //    this.ViewModel = new EditorViewModel(overlays);
        //    this.ViewModel.RegisterCloseAction(() => this.Close());
        //}

        //public EditorView(
        //    System.Collections.ObjectModel.ObservableCollection<Model.Overlay_Form> overlays, 
        //    int selectedIndex)
        //{
        //    InitializeComponent();

        //    this.ViewModel = new EditorViewModel(overlays, selectedIndex);
        //    this.ViewModel.RegisterCloseAction(() => this.Close());
        //}
    }
}
