using System;
using System.ComponentModel;
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
            }
        }

        /// <summary>
        /// Returns true if a window with this.WindowTitle exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                return Handle != IntPtr.Zero;
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

        /// <summary>
        /// Gets the handle of the window described by this ParentInfo.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (!TitleSpecified) return IntPtr.Zero;

                return FindWindow(null, WindowTitle);
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
        /// <param name="notify">Controls whether the OnPropertyChanged event is
        /// raised for Position on successful update.</param>
        /// <returns>The most recent position information of the window described by this
        /// ParentInfo.</returns>
        public Point CheckPosition(bool notify)
        {
            if(TitleSpecified)
            {
                IntPtr hWnd = this.Handle;
                if(hWnd != null)
                {
                    ParentInfo.Rect bounds = new ParentInfo.Rect();
                    if(GetWindowRect(hWnd, ref bounds))
                    {
                        this._Position.X = bounds.Left;
                        this._Position.Y = bounds.Top;
                        if (notify) OnPropertyChanged("Position");
                    }
                }
            }
            
            return Position;
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
            CheckPosition(false);

            return new Point
            (
                this.X + child_RelativePosition.X,
                this.Y + child_RelativePosition.Y
            );
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

        // TODOh Find a way to search windows without an exact match

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Ansi)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref ParentInfo.Rect rectangle);

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
