﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34014
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace begetInfo.Properties {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute("https://api.beget.ru/api/domain/getList?login=LOGINTEMPLATE&passwd=PASSWORDTEMPLA" +
            "TE&output_format=json")]
        public string request_sites {
            get {
                return ((string)(this["request_sites"]));
            }
            set {
                this["request_sites"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://api.beget.ru/api/stat/getSiteLoad?login=LOGINTEMPLATE&passwd=PASSWORDTEMP" +
            "LATE&output_format=json&input_format=json&input_data={%22site_id%22:SITEIDTEMPLA" +
            "TE}")]
        public string request_siteLoad {
            get {
                return ((string)(this["request_siteLoad"]));
            }
            set {
                this["request_siteLoad"] = value;
            }
        }
    }
}
