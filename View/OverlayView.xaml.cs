﻿using ScreenOverlayManager.Model;
using ScreenOverlayManager.ViewModel;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace ScreenOverlayManager
{
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

        private System.Windows.Threading.DispatcherTimer CheckParentTimer;

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
            this.ViewModel.Overlay.DraggingStarted += Overlay_DraggingStarted;
            this.ViewModel.Overlay.DraggingEnded   += Overlay_DraggingEnded;

            this.CheckParentTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, UpdateInterval),
                IsEnabled = !string.IsNullOrWhiteSpace(this.ViewModel.Overlay.ParentTitle)            
            };
            this.CheckParentTimer.Tick += (s, e) => UpdateOverlayPosition();

            this.SyncWindowLocation();
            this.Width  = ViewModel.Overlay.Width;
            this.Height = ViewModel.Overlay.Height;

        }

        private void Overlay_DraggingStarted(object sender)
        {
            this.CheckParentTimer.IsEnabled = false;

            this.ActivateHitTest();
        }

        private void Overlay_DraggingEnded(object sender)
        {
            this.DeactivateHitTest();

            Point   winPos      = new Point(this.Left, this.Top);
            Vector  newOffset   = ViewModel.Overlay.ParentInfo.GetChildOffset(winPos);

            ViewModel.Overlay.MoveTo(newOffset, true);

            CheckParentTimer.IsEnabled = true;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Extender.Debugging.Debug.WriteMessage
            (
                string.Format
                (
                    "OverlayView.ViewModel_PropertyChanged ({0})",
                    e.PropertyName
                ), 
                DEBUG
            );

            if      (e.PropertyName.Equals("X")) this.SyncWindowLocation();
            else if (e.PropertyName.Equals("Y")) this.SyncWindowLocation();
            else if (e.PropertyName.Equals("Width")) this.Width = ViewModel.Overlay.Width;
            else if (e.PropertyName.Equals("Height")) this.Height = ViewModel.Overlay.Height;
            else if (e.PropertyName.Equals("ParentTitle"))
            {
                this.CheckParentTimer.IsEnabled = ViewModel.Overlay.ParentInfo.TitleSpecified ?
                    true : false;

                this.SyncWindowLocation();
            }

            this.OverlayCanvas.InvalidateVisual();
        }

        // TODOh (Debug) find a way to have the overlay behave the same when minimizing and 
        //       closing the parent. Currently closing the parent keeps the window position static, 
        //       whereas minimizing switches positioning to be relative the desktop (0, 0). 
        //       I think I should pick one or the other for consistant behaviour.

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.DeactivateHitTest();
        }

        protected void SyncWindowLocation()
        {
            // no data to sync to 
            if (ViewModel == null) return;
            // Don't want to move the window around when the user is trying to place it
            if (ViewModel.Overlay.Draggable) return;


            this.Left   = ViewModel.Overlay.X;
            this.Top    = ViewModel.Overlay.Y;
        }

        protected void UpdateOverlayPosition()
        {
            // Store the overlay's current offset from parent
            Vector currentOffset = ViewModel.Overlay.OffsetFromParent;
            // Recheck parent position
            ViewModel.Overlay.ParentInfo.CheckPosition(true);
            // Move the overlay to its new position
            ViewModel.Overlay.MoveTo(currentOffset, true);
        }

        #region To delete...
        //protected void UpdateDraggable()
        //{
        //    if (JustMoved && this.ViewModel != null)
        //    {
        //        if (string.IsNullOrWhiteSpace(ViewModel.Overlay.ParentTitle))
        //        {
        //            // No parent specified
        //            this.ViewModel.Overlay.Move(this.Left, this.Top, true);
        //            //this.ViewModel.Overlay.X = this.Left;
        //            //this.ViewModel.Overlay.Y = this.Top;
        //        }
        //        else
        //        {
        //            // Parent is specified
        //            Point parentPos = GetParentWindowPosition();
        //            this.ViewModel.Overlay.Move
        //            (
        //                this.Left - parentPos.X,
        //                this.Top - parentPos.Y,
        //                true
        //            );
        //            //this.ViewModel.Overlay.X = this.Left - parentPos.X;
        //            //this.ViewModel.Overlay.Y = this.Top  - parentPos.Y;
        //        }

        //        JustMoved = !JustMoved;
        //    }

        //    if (ViewModel.Overlay.Draggable)
        //        this.ActivateHitTest();
        //    else
        //        this.DeactivateHitTest();
        //}

        //protected void PlaceWindow()
        //{
        //    if (ViewModel == null) return;
        //    // Don't want to move the window around when the user is trying to place it
        //    if (ViewModel.Overlay.Draggable) return; 

        //    if (!string.IsNullOrWhiteSpace(ViewModel.Overlay.ParentTitle))
        //    {
               
        //        Point parentPos = GetParentWindowPosition();
        //        this.Left = parentPos.X + ViewModel.Overlay.X;
        //        this.Top  = parentPos.Y + ViewModel.Overlay.Y;

        //        this.CheckParentTimer.IsEnabled = true;
        //    }
        //    else
        //    {
        //        this.Left = ViewModel.Overlay.X;
        //        this.Top  = ViewModel.Overlay.Y;

        //        this.CheckParentTimer.IsEnabled = false;
        //    }
        //}
        #endregion

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

        private int UpdateInterval
        {
            get
            {
                return Properties.Settings.Default.UpdateInterval;
            }
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
