using Extender.Windows;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Point = System.Windows.Point;

namespace ScreenOverlayManager.Model
{
    public sealed class Overlay
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
                return _ParentTitle;
            }
            set
            {
                _ParentTitle = value;
                OnPropertyChanged("ParentTitle");
            }
        }

        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
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
                return !string.IsNullOrWhiteSpace(this.ParentTitle);
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
                _Draggable = value;
                OnPropertyChanged("Draggable");
            }
        }

        #region Boxed properties

        private string  _Name;

        private string  _ParentTitle;

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

        public Overlay() { IsVisible = true; }

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
            this._ParentTitle       = parentWindow;
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
            this._X = position.X;
            this._Y = position.Y;
            this._Width = width;
            this._Height = height;
            this._Thickness = strokeThickness;
            this._Crosshair = drawCrosshair;
            this._PrimaryColor = primaryColor;
            this._SecondaryColor = secondaryColor;
            this._ParentTitle = parentWindow;
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

        #endregion
    }
}
