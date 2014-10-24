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
using ScreenOverlayManager.ViewModel;

namespace ScreenOverlayManager.View
{
    /// <summary>
    /// Interaction logic for AppConfigView.xaml
    /// </summary>
    public partial class AppConfigView : Window
    {
        AppConfigViewModel ViewModel
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

        public AppConfigView()
        {
            InitializeComponent();

            this.ViewModel = new AppConfigViewModel();
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Overlays"))
                this.OverlaysListBox.Items.Refresh();
        }
    }
}
