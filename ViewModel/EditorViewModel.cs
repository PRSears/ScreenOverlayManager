using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ScreenOverlayManager.Model;
using System.Windows.Input;
using Extender.WPF;

namespace ScreenOverlayManager.ViewModel
{
    public class EditorViewModel : Extender.WPF.ViewModel
    {
        //public Overlay_Form Overlay
        //{
        //    get
        //    {
        //        return Overlays[SelectedIndex];
        //    }
        //    set
        //    {
        //        Overlays[SelectedIndex] = value;
        //    }
        //}

        //public ObservableCollection<Overlay_Form> Overlays
        //{
        //    get
        //    {
        //        return _Overlays;
        //    }
        //    set
        //    {
        //        _Overlays = value;
        //        OnPropertyChanged("Overlays");
        //    }
        //}

        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        private int                                     _SelectedIndex;
        //private ObservableCollection<Overlay_Form>           _Overlays;
        //private ObservableCollection<OverlayWrapper>    _Backups;
        

        #region ICommands
        public ICommand LaunchPositionPickerCommand     { get; private set; }
        public ICommand AddOverlayCommand               { get; private set; }
        public ICommand SaveChangesCommand              { get; private set; }
        public ICommand CancelCommand                   { get; private set; }
        #endregion

        public EditorViewModel() { }

        //public EditorViewModel(ObservableCollection<Overlay_Form> overlays, int selectedIndex = 0)
        //{
        //    base.Initialize();

        //    this._SelectedIndex = selectedIndex;
        //    this._Overlays      = overlays;
        //    this._Backups       = CreateBackups(_Overlays);

        //    this.LaunchPositionPickerCommand = new RelayCommand(() => { throw new NotImplementedException(); });
                        
        //    //this.AddOverlayCommand

        //    // 
        //    // TODO Get rid of save button when editing an existing Overlay
        //    //      Disable cancel button when editing an existing Overlay
        //}

        //private ObservableCollection<OverlayWrapper> CreateBackups(ObservableCollection<Overlay_Form> overlays)
        //{
        //    ObservableCollection<OverlayWrapper> backups = new ObservableCollection<OverlayWrapper>();
        //    foreach(Overlay_Form o in overlays)
        //    {
        //        backups.Add(new OverlayWrapper(o));
        //    }

        //    return backups;
        //}

        //private void RevertBackups()
        //{
        //    Overlays.Clear();
        //    foreach(OverlayWrapper w in _Backups)
        //    {
        //        Overlays.Add(w.GetInner());
        //    }            
        //}

        public void AddOverlay()
        {

        }

        public void Cancel()
        {

        }
    }
}
