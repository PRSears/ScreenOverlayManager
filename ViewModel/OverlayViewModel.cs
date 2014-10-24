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
                OnPropertyChanged("Overlay");
            }
        }

        private Overlay _Overlay;


        public OverlayViewModel()
        {
            this._Overlay = new Overlay();
            this._Overlay.LoadDefaults();

            this.Initialize();
        }

        public OverlayViewModel(Overlay overlay)
        {
            this._Overlay = overlay;
            this.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Overlay.PropertyChanged += Overlay_PropertyChanged;
            //this._Draggable  = false;
        }

        private void Overlay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Overlay");
        }
    }
}
