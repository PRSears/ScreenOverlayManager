﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScreenOverlayManager.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int UpdateInterval {
            get {
                return ((int)(this["UpdateInterval"]));
            }
            set {
                this["UpdateInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("overlays")]
        public string SavedDirectory {
            get {
                return ((string)(this["SavedDirectory"]));
            }
            set {
                this["SavedDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public double DefaultOverlayWidth {
            get {
                return ((double)(this["DefaultOverlayWidth"]));
            }
            set {
                this["DefaultOverlayWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public double DefaultOverlayHeight {
            get {
                return ((double)(this["DefaultOverlayHeight"]));
            }
            set {
                this["DefaultOverlayHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public double DefaultOverlayStrokeThickness {
            get {
                return ((double)(this["DefaultOverlayStrokeThickness"]));
            }
            set {
                this["DefaultOverlayStrokeThickness"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FFFF0000")]
        public global::System.Windows.Media.Color DefaultOverlayColor1 {
            get {
                return ((global::System.Windows.Media.Color)(this["DefaultOverlayColor1"]));
            }
            set {
                this["DefaultOverlayColor1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FFFFFFFF")]
        public global::System.Windows.Media.Color DefaultOverlayColor2 {
            get {
                return ((global::System.Windows.Media.Color)(this["DefaultOverlayColor2"]));
            }
            set {
                this["DefaultOverlayColor2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DefaultOverlayDrawCrosshair {
            get {
                return ((bool)(this["DefaultOverlayDrawCrosshair"]));
            }
            set {
                this["DefaultOverlayDrawCrosshair"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200,200")]
        public global::System.Windows.Point DefaultOverlayPosition {
            get {
                return ((global::System.Windows.Point)(this["DefaultOverlayPosition"]));
            }
            set {
                this["DefaultOverlayPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DefaultOverlayDrawBorder {
            get {
                return ((bool)(this["DefaultOverlayDrawBorder"]));
            }
            set {
                this["DefaultOverlayDrawBorder"] = value;
            }
        }
    }
}
