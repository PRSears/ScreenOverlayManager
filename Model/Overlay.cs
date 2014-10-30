using Extender.Windows;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Point = System.Windows.Point;
using Vector = System.Windows.Vector;

namespace ScreenOverlayManager.Model
{
    public sealed class Overlay : INotifyPropertyChanged
    {
        public double Thickness
        {
            get
            {
                return _Thickness;
            }
            set
            {
                _Thickness = value;
                DimensionChanged();
            }
        }

        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
                DimensionChanged();
            }
        }

        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
                DimensionChanged();
            }
        }

        /// <summary>
        /// Gets or sets the X-Coordinate of the absolute position of this Overlay.
        /// Use this.MoveTo() if you're changing the position of the overlay. this.MoveTo()
        /// handles relative positioning when neccessary.
        /// </summary>
        public double X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// Gets or sets the Y-Coordinate of the absolute position of this Overlay.
        /// Use this.MoveTo() if you're changing the position of the overlay. this.MoveTo()
        /// handles relative positioning when neccessary.
        /// </summary>
        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                OnPropertyChanged("Y");
            }
        }

        [XmlIgnore]
        public Point Position
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets the vector offset of this overlay from a parent window.
        /// If no parent is specified (or the window isn't found) the offset is 
        /// relative to the desktop (0, 0). 
        /// </summary>
        [XmlIgnore]
        public Vector OffsetFromParent
        {
            get
            {
                if (ParentInfo.State == ParentInfo.ParentState.Open)
                    return this.Position - ParentInfo.Position;
                else 
                    return new Vector(X, Y);
            }
        }

        [XmlIgnore]
        public ParentInfo ParentInfo
        {
            get;
            set;
        }

        [XmlIgnore]
        public double OuterTop
        {
            get
            {
                return 0;
            }
        }

        [XmlIgnore]
        public double OuterLeft
        {
            get
            {
                return 0;
            }
        }

        [XmlIgnore]
        public double InnerTop
        {
            get
            {
                return Thickness;
            }
        }

        [XmlIgnore]
        public double InnerLeft
        {
            get
            {
                return Thickness;
            }
        }

        [XmlIgnore]
        public SolidColorBrush OuterStroke
        {
            get
            {
                return new SolidColorBrush(PrimaryColor);
            }
        }

        [XmlIgnore]
        public SolidColorBrush InnerStroke
        {
            get
            {
                return new SolidColorBrush(SecondaryColor);
            }
        }

        [XmlIgnore]
        public double OuterWidth
        {
            get
            {
                return Width;
            }
        }

        [XmlIgnore]
        public double OuterHeight
        {
            get
            {
                return Height;
            }
        }

        [XmlIgnore]
        public double InnerWidth
        {
            get
            {
                return Width - (Thickness * 2);
            }
        }

        [XmlIgnore]
        public double InnerHeight
        {
            get
            {
                return Height - (Thickness * 2);
            }
        }

        [XmlElement(Type=typeof(XmlColor))]
        public Color PrimaryColor
        {
            get
            {
                return _PrimaryColor;
            }
            set
            {
                _PrimaryColor = value;
                OnPropertyChanged("PrimaryColor");
                OnPropertyChanged("OuterStroke");
            }
        }

        [XmlElement(Type = typeof(XmlColor))]
        public Color SecondaryColor
        {
            get
            {
                return _SecondaryColor;
            }
            set
            {
                _SecondaryColor = value;
                OnPropertyChanged("SecondaryColor");
                OnPropertyChanged("InnerStroke");
            }
        }

        [XmlIgnore]
        public Cross Cross
        {
            get
            {
                return new Cross
                {
                    h_X1 = Thickness * 2,
                    h_Y1 = Height / 2,
                    h_X2 = Width - (Thickness * 2),
                    h_Y2 = Height / 2,

                    v_X1 = Width / 2,
                    v_Y1 = Thickness * 2,
                    v_X2 = Width / 2,
                    v_Y2 = Height - (Thickness * 2)
                };
            }
        }

        public bool DrawCrosshair
        {
            get
            {
                return _Crosshair;
            }
            set
            {
                _Crosshair = value;
                OnPropertyChanged("Crosshair");
            }
        }

        public bool DrawBorder
        {
            get
            {
                return _DrawBorder;
            }
            set
            {
                _DrawBorder = value;
                OnPropertyChanged("DrawBorder");
            }
        }

        [XmlIgnore]
        public System.Windows.Visibility BorderVisibility
        {
            get
            {
                return DrawBorder ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
            set
            {
                DrawBorder = (value.Equals(System.Windows.Visibility.Visible));
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string ParentTitle
        {
            get
            {
                return this.ParentInfo.WindowTitle;
            }
            set
            {
                this.ParentInfo.WindowTitle = value;
                OnPropertyChanged("ParentTitle");
            }
        }

        public bool IsVisible
        {
            get
            {
                return _IsVisible && (DrawBorder || DrawCrosshair);
            }
            set
            {
                _IsVisible = value;
                if (_IsVisible == true && !(DrawBorder || DrawCrosshair))
                    DrawBorder = true;
                OnPropertyChanged("IsVisible");
            }
        }     
   
        [XmlIgnore]
        public System.Windows.Visibility OverlayVisibility
        {
            get
            {
                return IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
            set
            {
                IsVisible = (value.Equals(System.Windows.Visibility.Visible));
            }
        }

        [XmlIgnore]
        public System.Windows.Visibility CrosshairVisibility
        {
            get
            {
                return DrawCrosshair ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
            set
            {
                DrawCrosshair = (value.Equals(System.Windows.Visibility.Visible));
            }
        }

        [XmlIgnore]
        public bool IsRelativePositioning
        {
            get
            {
                return ParentInfo.TitleSpecified;
            }
        }
        
        // can be ignored since the overlay will always be frozen when it first loads
        [XmlIgnore]
        public bool Draggable
        {
            get
            {
                return _Draggable;
            }
            set
            {
                bool prev = _Draggable;

                _Draggable = value;
                OnPropertyChanged("Draggable");

                if (prev == false && _Draggable == true)
                    OnDraggingStarted();
                else if (prev == true && _Draggable == false)
                    OnDraggingEnded();
            }
        }

        #region Boxed properties

        private string  _Name;

        //private string  _ParentTitle;

        private double  _Y;

        private double  _X;

        private double  _Height;

        private double  _Width;

        private double  _Thickness;

        private bool    _Crosshair;

        private bool    _DrawBorder;

        private Color   _SecondaryColor;

        private Color   _PrimaryColor;

        private bool    _IsVisible;

        private bool    _Draggable;

        #endregion

        public Overlay() 
        { 
            IsVisible = true;
            ParentInfo = new ParentInfo();
        }

        public Overlay
        (
            double x_position, 
            double y_position,
            double width,
            double height,
            double strokeThickness,
            bool drawCrosshair,
            Color primaryColor,
            Color secondaryColor,
            string parentWindow = ""
        ) : this()
        {
            this._X                 = x_position;
            this._Y                 = y_position;
            this._Width             = width;
            this._Height            = height;
            this._Thickness         = strokeThickness;
            this._Crosshair         = drawCrosshair;
            this._PrimaryColor      = primaryColor;
            this._SecondaryColor    = secondaryColor;

            this.ParentInfo.WindowTitle = parentWindow;
        }

        public Overlay
        (
            Point position,
            double width,
            double height,
            double strokeThickness,
            bool drawCrosshair,
            Color primaryColor,
            Color secondaryColor,
            string parentWindow = ""
        ) : this()
        {
            this._X                 = position.X;
            this._Y                 = position.Y;
            this._Width             = width;
            this._Height            = height;
            this._Thickness         = strokeThickness;
            this._Crosshair         = drawCrosshair;
            this._PrimaryColor      = primaryColor;
            this._SecondaryColor    = secondaryColor;

            this.ParentInfo.WindowTitle = parentWindow;
        }

        /// <summary>
        /// Uses a streamwriter to serialize this Overlay object to an XML file.
        /// </summary>
        public void Serialize(System.IO.StreamWriter outStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Overlay));

            serializer.Serialize(outStream, this);
        }

        /// <summary>
        /// Tries to deserialize an Overlay object from the FileStream.
        /// Returns a default overlay if deserialization fails.
        /// </summary>
        public static Overlay TryDeserialize(System.IO.FileStream fromFile)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Overlay));
                return (Overlay)deserializer.Deserialize(fromFile);
            }
            catch (Exception e)
            {
                Extender.Debugging.ExceptionTools.WriteExceptionText
                (
                    e, 
                    true,
                    "Overlay.Deserialize encountered an error while attempting to deserialize."
                );

                return new Overlay();
            }
        }

        /// <summary>
        /// Deserializes an Overlay object from the the FileStream.
        /// Throws an exception on failure.
        /// </summary>
        public static Overlay Deserialize(System.IO.FileStream fromFile)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Overlay));
            return (Overlay)deserializer.Deserialize(fromFile);
        }

        /// <summary>
        /// Loads default settings from the Settings.settings file. Overwrites
        /// any existing properties.
        /// </summary>
        public void LoadDefaults()
        {
            this.X              = Properties.Settings.Default.DefaultOverlayPosition.X;
            this.Y              = Properties.Settings.Default.DefaultOverlayPosition.Y;
            this.Width          = Properties.Settings.Default.DefaultOverlayWidth;
            this.Height         = Properties.Settings.Default.DefaultOverlayHeight;
            this.Thickness      = Properties.Settings.Default.DefaultOverlayStrokeThickness;
            this.PrimaryColor   = Properties.Settings.Default.DefaultOverlayColor1;
            this.SecondaryColor = Properties.Settings.Default.DefaultOverlayColor2;
            this.DrawCrosshair  = Properties.Settings.Default.DefaultOverlayDrawCrosshair;
            this.DrawBorder     = Properties.Settings.Default.DefaultOverlayDrawBorder;
            this.Name           = string.Empty;
            this.ParentTitle    = string.Empty;
        }

        /// <summary>
        /// Edits the properties of this Overlay to match those of Overlay b.
        /// </summary>
        /// <param name="b">Second Overlay object whose properties are being copied.</param>
        public void CopyFrom(Overlay b)
        {
            this.Name           = b.Name;
            this.X              = b.X;
            this.Y              = b.Y;
            this.Width          = b.Width;
            this.Height         = b.Height;
            this.PrimaryColor   = b.PrimaryColor;
            this.SecondaryColor = b.SecondaryColor;
            this.DrawBorder     = b.DrawBorder;
            this.DrawCrosshair  = b.DrawCrosshair;
            this.ParentTitle    = b.ParentTitle;
            this.IsVisible      = b.IsVisible;
        }

        /// <summary>
        /// Changes the X and Y coordinates of this overlay. If a ParentTitle has been specified - 
        /// and the parent exists - the Overlay will be positioned relative to the parent.
        /// </summary>
        /// <param name="x">New X coordinate. If a ParentTitle has been specified - 
        /// and the parent exists - the Overlay will be positioned relative to the parent.</param>
        /// <param name="y">New Y coordinate. If a ParentTitle has been specified - 
        /// and the parent exists - the Overlay will be positioned relative to the parent.</param>
        /// <param name="relative">
        /// When true the move will attempt to be relative to the parent (if present).
        /// </param>
        /// <param name="quiet">When true the OnPropertyChanged event is not
        /// raised for this.X or this.Y.</param>
        public void MoveTo(double x, double y, bool relative, bool quiet = false)
        {
            this.MoveTo(new Vector(X, Y), relative, quiet);
        }

        /// <summary>
        /// Changes the X and Y coordinates of this overlay. If a ParentTitle has been specified - 
        /// and the parent exists - the Overlay will be positioned relative to the parent.
        /// </summary>
        /// <param name="newOffset">
        /// A vector representing the difference between the parent window (or Point(0, 0) if not 
        /// specified) and the new position of the overlay.
        /// </param>
        /// <param name="relative">
        /// When true the move will attempt to be relative to the parent (if present).</param>
        /// <param name="quiet">When true the OnPropertyChanged event is not
        /// raised for this.X or this.Y.</param>
        public void MoveTo(Vector newOffset, bool relative, bool quiet = false)
        {
            Point newAbsPos;

            if (relative && ParentInfo.State == ParentInfo.ParentState.Open)
                newAbsPos = ParentInfo.GetChildAbsolutePosition(newOffset);
            else
                newAbsPos = (Point)newOffset;

            if (!newAbsPos.Equals(this.Position))
            {
                if (quiet)
                {
                    this._X = newAbsPos.X;
                    this._Y = newAbsPos.Y;
                }
                else
                {
                    this.X = newAbsPos.X;
                    this.Y = newAbsPos.Y;
                }
            }
        }

        public override string ToString()
        {
            return string.Format
                (
                    @"Overlay ""{0}"" [ @({1},{2}) - {3} & {4} - {5} ]",
                    this.Name,
                    this.X, this.Y,
                    this.PrimaryColor.ToString(),
                    this.SecondaryColor.ToString(),
                    this.IsVisible ? "Visible" : "Hidden"
                );
        }

        public override bool Equals(object obj)
        {
            if (obj is Overlay)
            {
                Overlay b = (Overlay)obj;

                return (this.Name == b.Name) &&
                       (this.X == b.X) &&
                       (this.Y == b.Y) &&
                       (this.Width == b.Width) &&
                       (this.Height == b.Height) &&
                       (this.PrimaryColor.Equals(b.PrimaryColor)) &&
                       (this.SecondaryColor.Equals(b.SecondaryColor)) &&
                       (this.DrawBorder == b.DrawBorder) &&
                       (this.DrawCrosshair == b.DrawCrosshair) &&
                       (this.ParentTitle == b.ParentTitle) && 
                       (this.IsVisible == b.IsVisible);
            }
            else return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event DraggingStartEventHandler DraggingStarted;
        public event DraggingEndEventHandler DraggingEnded;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnDraggingStarted()
        {
            DraggingStartEventHandler handler = DraggingStarted;

            if(handler != null)
            {
                handler(this);
            }
            Extender.Debugging.Debug.WriteMessage("OnDraggingStarted.",
                Properties.Settings.Default.Debugging);
        }

        private void OnDraggingEnded()
        {
            DraggingEndEventHandler handler = DraggingEnded;

            if(handler != null)
            {
                handler(this);
            }
            Extender.Debugging.Debug.WriteMessage("OnDraggingEnded.",
                Properties.Settings.Default.Debugging);
        }

        private void DimensionChanged()
        {
            OnPropertyChanged("Width");
            OnPropertyChanged("Height");
            OnPropertyChanged("Thickness");
            OnPropertyChanged("OuterWidth");
            OnPropertyChanged("OuterHeight");
            OnPropertyChanged("InnerTop");
            OnPropertyChanged("InnerLeft");
            OnPropertyChanged("InnerWidth");
            OnPropertyChanged("InnerHeight");
        }
    }

    public delegate void DraggingStartEventHandler(object sender);
    public delegate void DraggingEndEventHandler(object sender);
}
