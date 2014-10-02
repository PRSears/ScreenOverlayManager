using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extender.Debugging;
using HwndObject = WindowScrape.Types.HwndObject;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Rectangle = System.Windows.Rect;
using ScreenOverlayManager.Model;

namespace ScreenOverlayManager.ViewModel
{
    public class OverlayViewModel : Extender.WPF.ViewModel
    {
        public Overlay Overlay
        {
            get
            {
                return _Overlay;
            }
            set
            {
                _Overlay = value;
                OnPropertyChanged("Properties");
            }
        }

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


        #region Boxed Properties
        private bool    _Draggable;
        private Overlay _Overlay;
        #endregion


        #region ICommands

        #endregion


        public OverlayViewModel()
        {
            this.Initialize();

            this._Overlay = new Overlay();
            this.LoadDefaults();
        }

        public OverlayViewModel(Overlay overlay)
        {
            this.Initialize();

            this._Overlay = overlay;
        }

        public override void Initialize()
        {
            base.Initialize();

            this._Draggable  = false;
        }

        public void LoadDefaults()
        {
            this.Overlay.X              = Properties.Settings.Default.DefaultOverlayPosition.X;
            this.Overlay.Y              = Properties.Settings.Default.DefaultOverlayPosition.Y;
            this.Overlay.Width          = Properties.Settings.Default.DefaultOverlayWidth;
            this.Overlay.Height         = Properties.Settings.Default.DefaultOverlayHeight;
            this.Overlay.Thickness      = Properties.Settings.Default.DefaultOverlayStrokeThickness;
            this.Overlay.PrimaryColor   = Properties.Settings.Default.DefaultOverlayColor1;
            this.Overlay.SecondaryColor = Properties.Settings.Default.DefaultOverlayColor2;
            this.Overlay.DrawCrosshair  = Properties.Settings.Default.DefaultOverlayDrawCrosshair;
            this.Overlay.DrawBorder     = Properties.Settings.Default.DefaultOverlayDrawBorder;
            this.Overlay.Name           = string.Empty;
            this.Overlay.ParentTitle    = string.Empty;
        }
    }
}
