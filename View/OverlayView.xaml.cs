using ScreenOverlayManager.Model;
using ScreenOverlayManager.ViewModel;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace ScreenOverlayManager
{
    // TODOh Implement relative positioning to parent window (matching 'ParentTitle')

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

        private bool JustMoved { get; set; }

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
            this.ViewModel.Overlay.PropertyChanged += ViewModel_PropertyChanged;

            this.Left   = ViewModel.Overlay.X;
            this.Top    = ViewModel.Overlay.Y;
            this.Width  = ViewModel.Overlay.Width;
            this.Height = ViewModel.Overlay.Height;

            //var trayMenu = (ContextMenu)this.Resources["SysTrayMenu"];
            //trayMenu.Items.Add();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Extender.Debugging.Debug.WriteMessage
            (
                string.Format
                (
                    "OverlayView ViewModel_PropertyChanged being handled for {0}.",
                    e.PropertyName
                ), 
                DEBUG
            );

            if (e.PropertyName.Equals("Draggable"))
                this.UpdateDraggable();
            else if (e.PropertyName.Equals("X"))
                this.Left = ViewModel.Overlay.X;
            else if (e.PropertyName.Equals("Y"))
                this.Top = ViewModel.Overlay.Y;
            else if (e.PropertyName.Equals("Width"))
                this.Width = ViewModel.Overlay.Width;
            else if (e.PropertyName.Equals("Height"))
                this.Height = ViewModel.Overlay.Height;

            this.OverlayCanvas.InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
                this.JustMoved = true;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.UpdateDraggable();
        }

        protected void UpdateDraggable()
        {
            if (JustMoved && this.ViewModel != null)
            {
                this.ViewModel.Overlay.X = this.Left;
                this.ViewModel.Overlay.Y = this.Top;

                JustMoved = !JustMoved;
            }

            if (ViewModel.Overlay.Draggable)
                this.ActivateHitTest();
            else
                this.DeactivateHitTest();
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

        private bool DEBUG
        {
            get
            {
                return Properties.Settings.Default.Debugging;
            }
        }
    }
}
