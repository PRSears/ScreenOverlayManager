﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using ScreenOverlayManager.ViewModel;
using ScreenOverlayManager.Model;

namespace ScreenOverlayManager.View
{
    /// <summary>
    /// Interaction logic for NewOverlayView.xaml
    /// </summary>
    public partial class EditorView : Window
    {
        private EditorViewModel ViewModel
        {
            get
            {
                if (DataContext is EditorViewModel)
                    return (EditorViewModel)DataContext;
                else
                    return null;
            }
            set
            {
                DataContext = value;
            }
        }

        public EditorView()
        {
            ViewModel = new EditorViewModel();
            Init();
        }

        public EditorView(Overlay existingOverlay)
        {
            ViewModel = new EditorViewModel(existingOverlay);
            Init();
        }

        protected void Init()
        {
            InitializeComponent();
            ViewModel.RegisterCloseAction(this.Close);
        }
    }
}
