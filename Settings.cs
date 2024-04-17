using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace USALauncher;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.6.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
    private static Settings defaultInstance = (Settings)Synchronized(new Settings());

    public static Settings Default => defaultInstance;

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string armaPath
    {
        get
        {
            return (string)this["armaPath"];
        }
        set
        {
            this["armaPath"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool UpgradeRequired
    {
        get
        {
            return (bool)this["UpgradeRequired"];
        }
        set
        {
            this["UpgradeRequired"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string profile
    {
        get
        {
            return (string)this["profile"];
        }
        set
        {
            this["profile"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool noSplash
    {
        get
        {
            return (bool)this["noSplash"];
        }
        set
        {
            this["noSplash"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool windowed
    {
        get
        {
            return (bool)this["windowed"];
        }
        set
        {
            this["windowed"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int maxVram
    {
        get
        {
            return (int)this["maxVram"];
        }
        set
        {
            this["maxVram"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string customParams
    {
        get
        {
            return (string)this["customParams"];
        }
        set
        {
            this["customParams"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string profilePath
    {
        get
        {
            return (string)this["profilePath"];
        }
        set
        {
            this["profilePath"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool noLogs
    {
        get
        {
            return (bool)this["noLogs"];
        }
        set
        {
            this["noLogs"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool enableHT
    {
        get
        {
            return (bool)this["enableHT"];
        }
        set
        {
            this["enableHT"] = value;
        }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool skipIntro
    {
        get
        {
            return (bool)this["skipIntro"];
        }
        set
        {
            this["skipIntro"] = value;
        }
    }

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }
}
