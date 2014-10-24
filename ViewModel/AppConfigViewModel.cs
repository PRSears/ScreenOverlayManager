﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ScreenOverlayManager.Model;
using Extender.WPF;
using Extender;
using System.Windows.Input;
using System.IO;
using Debug = Extender.Debugging.Debug;

namespace ScreenOverlayManager.ViewModel
{
    public class AppConfigViewModel : Extender.WPF.ViewModel
    {
        // TODO Implement settings editor
        // TODO Implement minimize to tray / close all windows on exit / etc

        // TODO decide when to call SaveState()

        public ObservableCollection<Checkable<Overlay>> Overlays
        {
            get
            {
                return _Overlays;
            }
            set
            {
                _Overlays = value;
                OnPropertyChanged("Overlays");
            }
        }
        private ObservableCollection<Checkable<Overlay>> _Overlays;

        public ICommand ImportCommand       { get; private set; }
        public ICommand ExportCommand       { get; private set; }
        public ICommand EditCommand         { get; private set; }
        public ICommand QuickPosCommand     { get; private set; }
        public ICommand ShowHideCommand     { get; private set; }
        public ICommand DeleteCommand       { get; private set; }
        public ICommand CreateNewCommand    { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }

        protected WindowManager WindowManager;

        public AppConfigViewModel()
        {
            this.Overlays       = new ObservableCollection<Checkable<Overlay>>();
            this.WindowManager  = new WindowManager();

            LoadState();

            ImportCommand   = new RelayCommand(() => ImportFromFile());
            ExportCommand   = new RelayCommand(() => ExportSelectedToFile(), () => HasSelected);
            EditCommand     = new RelayCommand(() => Edit(Selected), () => HasSelected);

            QuickPosCommand = new RelayCommand
            (
                () =>
                {
                    if(HasSelected)
                        Selected.Draggable = !SelectedIsDraggable;
                    OnPropertyChanged("QuickPosButtonText");
                },

                () => HasSelected
            );

            ShowHideCommand = new RelayCommand
            (
                () => 
                {
                    Selected.IsVisible = !Selected.IsVisible;
                    OnPropertyChanged("ShowHideButtonText");
                },
                () => HasSelected
            );

            DeleteCommand   = new RelayCommand
            (
                () => DeleteSelected(),
                () => HasSelected
            );

            CreateNewCommand = new RelayCommand
            (
                () => CreateNewOverlay(true)
            );

            OpenSettingsCommand = new RelayCommand
            (
                () => OpenSettingsDialog()
            );
        }

        public void CreateNewOverlay(bool openEditor)
        {
            Overlay n = new Overlay();
            n.LoadDefaults();

            OpenExistingOverlay(n);

            if (openEditor)
                Edit(n);
        }

        public void OpenExistingOverlay(Overlay overlay)
        {
            overlay.PropertyChanged += Overlay_PropertyChanged;
            _Overlays.Add(new Checkable<Overlay>(overlay));

            // Try to subscribe to the PropertyChanged event for the new Checkable<Overlay>
            try
            {
                _Overlays.First(co => co.Resource.Equals(overlay))
                         .PropertyChanged += Overlays_SelectionChanged;
            }
            catch
            {
                Debug.WriteMessage("Failed to subscribe to new Checkable<Overlay>'s PropertyChanged event.", DEBUG, "warn");
            }

            WindowManager.OpenWindow(new OverlayView(overlay), true);
        }

        public void DeleteSelected()
        {
            if (!HasSelected) return;
            this.DeleteOverlay(Selected);
        }

        protected void DeleteOverlay(Overlay overlay)
        {
            //
            // Removes the first listing in Overlays collection with a matching Overlay contained in it.
            Debug.WriteMessage("Calling DeleteOverlay.", DEBUG);
            Debug.WriteMessage
            (
                Overlays.RemoveFirst(co => co.Resource.Equals(overlay)).ToString(),
                DEBUG
            );

            //
            // Uses WindowManager to close the corresponding OverlayView window.
            WindowManager.CloseChild
            (
                WindowManager.Children.Where(w => w.GetType() == typeof(OverlayView))
                                      .Cast<OverlayView>()
                                      .FirstOrDefault(ov => ov.ViewModel.Overlay.Equals(overlay))
            );            
        }

        private void Overlays_SelectionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteMessage(string.Format("Overlays_SelectionChanged Checkable<Overlay>.({0})", e.PropertyName), DEBUG);

            // If the selected item changes we need to refresh the context-sensitive buttons' text
            OnPropertyChanged("ShowHideButtonText");
            OnPropertyChanged("QuickPosButtonText");
        }

        private void Overlay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Overlays");

            if (e.PropertyName.Equals("IsVisible"))
                OnPropertyChanged("ShowHideButtonText");
            else if (e.PropertyName.Equals("Draggable"))
                OnPropertyChanged("QuickPosButtonText");
        }

        public void ImportFromFile()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = @"XML documents (*.txt, *.xml)
                |*.txt;*.xml|All files (*.*)|*.*";

            bool? clickedOK = dialog.ShowDialog();
            if (clickedOK == true)
            {
                try
                {
                    Overlays.Add(new Checkable<Overlay>(OverlayFromFile(dialog.FileName)));
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("The selected file could not be imported.\n" +
                        "Make sure the file is a properly formatted Overlay XML file.");
                }
            }
        }

        protected Overlay OverlayFromFile(string pathToFile)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream
                (
                    pathToFile,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );

                return Overlay.Deserialize(fileStream);
            } 
            finally
            {
                if (fileStream != null) fileStream.Dispose();
            }
        }

        public void ExportSelectedToFile()
        {
            if (this.Selected == null) return;

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = "Export overlay to XML...";
            dialog.DefaultExt = ".xml";
            dialog.FileName = string.Format("{0}.xml", this.Selected.Name);

            bool? UserClickedSave = dialog.ShowDialog();
            if (UserClickedSave == true)
            {
                if (File.Exists(dialog.FileName))
                    File.Delete(dialog.FileName);

                SaveOverlay(Selected, dialog.FileName);
            }
        }

        protected void Edit(Overlay overlay)
        {
            if (overlay == null) return;

            WindowManager.OpenWindow(new View.EditorView(overlay), true);
        }

        public bool SelectedIsDraggable
        {
            get
            {
                if (Selected == null) return false;

                return Selected.Draggable;
            }
        }

        public bool HasSelected
        {
            get
            {
                if (Overlays == null)   return false;
                if (Overlays.Count < 1) return false;

                return Overlays.Count(o => o.IsChecked) > 0;
            }
        }

        public Overlay Selected
        {
            get
            {
                if (Overlays == null) return null;

                var sel = Overlays.FirstOrDefault(o => o.IsChecked);

                return sel == null ? null : sel.Resource;
            }
        }

        public bool SelectedIsVisible
        {
            get
            {
                if (!HasSelected) return false;

                return Selected.IsVisible;
            }
        }

        public string QuickPosButtonText
        {
            get
            {
                if (!HasSelected) return "Quick Positioner";
                return SelectedIsDraggable ? "End Quick Pos" : "Quick Positioner";
            }
        }

        public string ShowHideButtonText
        {
            get
            {
                //default text should be hide, since that is the most common behavior
                if (!HasSelected) return "Hide";

                return SelectedIsVisible ? "Hide" : "Show";
            }
        }

        protected void LoadState()
        {
            foreach(string file in Directory.GetFiles(Properties.Settings.Default.SavedDirectory))
            {
                try
                {
                    OpenExistingOverlay(OverlayFromFile(file));
                }
                catch
                {
                    Debug.WriteMessage
                        (
                            string.Format(@"File at ""{0}"" could not be loaded.", file), 
                            DEBUG
                        );
                }
            }
        }

        public void SaveState()
        { 
            // Ensure the directory exists
            if(!Directory.Exists(SavedOverlaysPath))
                Directory.CreateDirectory(SavedOverlaysPath);
            // Empty the directory 
            if(!Directory.EnumerateFileSystemEntries(SavedOverlaysPath).Any())
            {
                foreach(string file in Directory.EnumerateFiles(SavedOverlaysPath))
                {
                    File.Delete(file);
                }
            }

            for(int i = 1; i <= Overlays.Count; i++)
            {
                string filename = Path.Combine
                (
                    SavedOverlaysPath,
                    string.Format(OverlayNameFormat, i.ToString("D3"))
                );

                SaveOverlay(Overlays[i].Resource, filename);
            }
        }

        protected void SaveOverlay(Overlay overlay, string fullPath)
        {
            StreamWriter stream = null;
            try
            {
                stream = File.CreateText(fullPath);
                overlay.Serialize(stream);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show
                    (
                        string.Format("A problem was encountered while saving the current " +
                                      "overlay states.\n\n{0}\nFor {1}",
                                      Extender.Debugging.ExceptionTools.CreateExceptionText(e, false),
                                      overlay.ToString())
                    );

                if (DEBUG) Extender.Debugging.ExceptionTools.WriteExceptionText(e, true);
            }
            finally
            {
                stream.Dispose();
            }
        }

        protected void OpenSettingsDialog()
        {
            Debug.WriteMessage("Settings dialog called.", DEBUG);
            
            System.Windows.Forms.MessageBox.Show("Not yet implemented.");
        }

        #region //Settings.Settings aliases
        private bool DEBUG
        {
            get
            {
                return Properties.Settings.Default.Debugging;
            }
        }

        private string SavedOverlaysPath
        {
            get
            {
                return Properties.Settings.Default.SavedDirectory;
            }
        }

        private string OverlayNameFormat
        {
            get
            {
                return Properties.Settings.Default.DefaultOverlayFilenameFormat;
            }
        }
        #endregion
    }
}