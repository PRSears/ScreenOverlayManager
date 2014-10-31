using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace ScreenOverlayManager.Model
{
    public class ParentInfo : INotifyPropertyChanged
    {
        public enum ParentState
        {
            NotFound    = 0,
            Open        = 1,
            Minimized   = 2
        }

        private static Point MinimizedPosition = new Point(-32000, -32000);

        /// <summary>
        /// Gets or sets the string used to search for matching window titles.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return _WindowTitle;
            }
            set
            {
                _WindowTitle = value;
                OnPropertyChanged("WindowTitle");

                // will force a re-check next time Handle's getter is called
                Handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Returns true if a window with this.WindowTitle exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                return TitleSpecified && IsWindow(_Handle);
            }
        }

        /// <summary>
        /// Returns true if this.WindowTitle contains a value. (Not null, whitespace, or empty)
        /// </summary>
        public bool TitleSpecified
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.WindowTitle);
            }
        }

        /// <summary>
        /// Makes an educated guess at what state the parent is in.
        /// Not found / Minimized / Open.
        /// </summary>
        public ParentState State
        {
            get
            {
                if(!Exists)
                    return ParentState.NotFound;
                else if (Position.Equals(MinimizedPosition))
                    return ParentState.Minimized;
                else
                    return ParentState.Open;
            }
        }

        /// <summary>
        /// Gets the most recently polled position of the Window described by this
        /// ParentInfo object. The point (0, 0) is returned if no matching window was 
        /// found.
        /// </summary>
        public Point Position
        {
            get
            {
                if (!TitleSpecified)
                    QuietSetCoords(0, 0);

                return _Position;
            }
            private set
            {
                _Position = value;
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// Gets the X coordinate of this.Position.
        /// </summary>
        public double X
        {
            get
            {
                return _Position.X;
            }
            private set
            {
                _Position.X = value;
                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// Gets the Y coordinate of this.Position.
        /// </summary>
        public double Y
        {
            get
            {
                return _Position.Y;
            }
            private set
            {
                _Position.Y = value;
                OnPropertyChanged("Y");
            }
        }

        private Point   _Position;
        private string  _WindowTitle;
        private IntPtr  _Handle;

        /// <summary>
        /// Gets the handle of the window described by this ParentInfo.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (!IsWindow(_Handle))
                    CheckHandle();

                return _Handle;
            }
            private set
            {
                _Handle = value;
                OnPropertyChanged("Handle");
            }
        }

        public ParentInfo(string windowTitle)
        {
            this.WindowTitle = windowTitle;
            this.Position = new Point();
        }

        public ParentInfo() : this(string.Empty) { }

        /// <summary>
        /// Rechecks the position of the window described by this ParentInfo.
        /// </summary>
        /// <param name="notify">Controls whether the PropertyChanged event is
        /// raised for Position on successful update.</param>
        /// <returns>The most recent position information of the window described by this
        /// ParentInfo.</returns>
        public Point CheckPosition(bool notify)
        {
            ParentInfo.Rect bounds = new ParentInfo.Rect();

            if(Exists && GetWindowRect(_Handle, ref bounds))
            {
                _Position.X = bounds.Left;
                _Position.Y = bounds.Top;
            }
            else
            {
                _Position.X = 0;
                _Position.Y = 0;
                CheckHandle(); // Can try again on the next pass
            }

            if (notify) OnPropertyChanged("Position");
            return Position;
        }

        protected IntPtr CheckHandle()
        {
            if (!TitleSpecified) return IntPtr.Zero;

            IntPtr hWnd = FindWindow(null, WindowTitle);

            if (hWnd == IntPtr.Zero) // no exact match
            {
                var proc = Process.GetProcesses()
                                  .FirstOrDefault(p => p.MainWindowTitle.Contains(WindowTitle));

                if (proc == default(Process)) hWnd = IntPtr.Zero;
                else hWnd = proc.MainWindowHandle;
            }

            Handle = hWnd;

            return _Handle;
        }

        /// <summary>
        /// Calculates the absolute position of a child from position information
        /// relative to the window described by this ParentInfo.
        /// </summary>
        /// <param name="child_RelativePosition">Relative position of the child whose 
        /// absolute position is being calculated.</param>
        /// <returns>The absolute position of the child described by
        /// child_RelativePosition.</returns>
        public Point GetChildAbsolutePosition(Point child_RelativePosition)
        {
            CheckPosition(false);

            return new Point
            (
                this.X + child_RelativePosition.X,
                this.Y + child_RelativePosition.Y
            );
        }
        
        /// <summary>
        /// Calculates the absolute position of a child from position information
        /// relative to the window described by this ParentInfo.
        /// </summary>
        /// <param name="child_RelativePosition">Offset of child position from the parent,
        /// for the child whose absolute position is being calculated.</param>
        /// <returns>The absolute position of the child described by
        /// child_RelativePosition.</returns>
        public Point GetChildAbsolutePosition(Vector child_RelativePosition)
        {
            return GetChildAbsolutePosition((Point)child_RelativePosition);
        }

        /// <summary>
        /// Calculates the offset of a child from the parent.
        /// </summary>
        /// <param name="child_AbsolutePosition">The absolute position of the child
        /// whose offset is being calculated.</param>
        /// <returns>Vector offset of the child from this parent.</returns>
        public Vector GetChildOffset(Point child_AbsolutePosition)
        {
            CheckPosition(false);

            if (this.State == ParentInfo.ParentState.Open)
                return child_AbsolutePosition - this.Position;
            else
                return (Vector)child_AbsolutePosition;
        }

        /// <summary>
        /// Calculates the offset of a child from the parent.
        /// </summary>
        /// <param name="x">The x-coordinate of the absolute position of the child
        /// whose offset is being calculated.</param>
        /// <param name="y">The y-coordinate of the absolute position of the child
        /// whose offset is being calculated.</param>
        /// <returns>Vector offset of the child from this parent.</returns>
        public Vector GetChildOffset(double x, double y)
        {
            return GetChildOffset(new Point(x, y));
        }

        private void QuietSetCoords(double x, double y)
        {
            _Position.X = x;
            _Position.Y = y;
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Ansi)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref ParentInfo.Rect rectangle);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        private struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
