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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScreenOverlayManager.Model;
using ScreenOverlayManager.ViewModel;
using System.Runtime.InteropServices;

namespace ScreenOverlayManager
{

    //
    // TODOh Re-write
    // 
    // Stop using a windows form to draw the overlay... switch to using my Extender's 
    // Window manager with a WPF window. 
    //
    // Create a settings editor window as the parent / base window. Have all the overlays
    // managed as children of that window.
    //
    // have a toggle mode so the user can move each individual overlay by dragging,
    // then have it toggle back to being click through (IsHitTestVisible="False")
    // 
    // Positions will be absolute by default. can set a parent window in settings (somehow...)
    // and position will be recorded as relative to that.
    //      - Get parent window location, subtract overlay (absolute) position and use
    //        that value to update overlay position on next tick
    //
    // Main window (whatever that ends up being) should control a timer to keep track of overlay 
    // updates. 
    //
    // User leaving edit mode on an overlay triggers saving (serialization)

    /// <summary>
    /// Interaction logic for OverlayView.xaml
    /// </summary>
    public partial class OverlayView : Window
    {
        public OverlayViewModel ViewModel
        {
            get
            {
                if (DataContext is OverlayViewModel)
                    return (OverlayViewModel)DataContext;
                else
                    return null;
            }
            set
            {
                DataContext = value;
            }
        }

        public OverlayView()
        {
            InitializeComponent();

            this.ViewModel = new OverlayViewModel();
            this.InitWindow();
        }

        public OverlayView(Overlay overlay)
        {
            InitializeComponent();

            this.ViewModel = new OverlayViewModel(overlay);
            this.InitWindow();
        }

        protected void InitWindow()
        {
            this.ViewModel.RegisterCloseAction(() => this.Close());
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            //var trayMenu = (ContextMenu)this.Resources["SysTrayMenu"];
            //trayMenu.Items.Add();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Draggable"))
                this.UpdateDraggable();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if(e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.UpdateDraggable();
        }
        
        /// <summary>
        /// Uses a WindowsInteropHelper to get the Handle of this WPF Window. 
        /// [Read-Only]
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return new System.Windows.Interop.WindowInteropHelper(this).Handle;
            }
        }

        protected void UpdateDraggable()
        {
            if (ViewModel.Draggable)
                this.ActivateHitTest();
            else
                this.DeactivateHitTest();
        }

        protected void DeactivateHitTest()
        {
            SetWindowLong
                (
                    this.Handle, 
                    GWL.ExStyle,
                    GetWindowLong(this.Handle, GWL.ExStyle) | (int)WS_EX.Transparent | (int)WS_EX.Layered
                );
        }

        protected void ActivateHitTest()
        {
            SetWindowLong
                (
                    this.Handle,
                    GWL.ExStyle,
                    GetWindowLong(this.Handle, GWL.ExStyle) & ~(int)WS_EX.Transparent & ~(int)WS_EX.Layered
                );
        }

        #region Fucking bullshit
        // IsHitTestVisible=false doesn't seem to work on elements drawn to a canvas...
        // So all this bullshit is still required! What the fuck is the point of turning off
        // hit tests if it doesn't work for all elements drawn on the widnow?


        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        //http://msdn.microsoft.com/en-us/library/windows/desktop/ff700543%28v=vs.85%29.aspx

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);
        #endregion
    }
}
