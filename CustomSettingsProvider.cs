using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

public class CustomSettingsProvider : SettingsProvider
{
    public override string ApplicationName
    {
        get => Application.ProductName;
        set { /* Do nothing */ }
    }

    public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
    {
        var values = new SettingsPropertyValueCollection();
        foreach (SettingsProperty property in properties)
        {
            var value = new SettingsPropertyValue(property)
            {
                IsDirty = false,
                SerializedValue = GetValue(property)
            };
            values.Add(value);
        }
        return values;
    }

    public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
    {
        foreach (SettingsPropertyValue value in values)
        {
            SetValue(value);
        }
    }

    public override string Name => "CustomSettingsProvider";

    public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
    {
        base.Initialize(this.Name, config);
    }

    private string GetSettingsFilename()
    {
        string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        return Path.Combine(directory, "user.config");
    }

    private string GetValue(SettingsProperty property)
    {
        string filename = GetSettingsFilename();
        if (File.Exists(filename))
        {
            var xml = File.ReadAllText(filename);
            var settings = new ExeConfigurationFileMap { ExeConfigFilename = filename };
            var config = ConfigurationManager.OpenMappedExeConfiguration(settings, ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[property.Name] != null)
            {
                return config.AppSettings.Settings[property.Name].Value;
            }
        }
        return property.DefaultValue?.ToString();
    }

    private void SetValue(SettingsPropertyValue value)
    {
        string filename = GetSettingsFilename();
        var settings = new ExeConfigurationFileMap { ExeConfigFilename = filename };
        var config = ConfigurationManager.OpenMappedExeConfiguration(settings, ConfigurationUserLevel.None);

        if (config.AppSettings.Settings[value.Name] != null)
        {
            config.AppSettings.Settings[value.Name].Value = value.SerializedValue?.ToString();
        }
        else
        {
            config.AppSettings.Settings.Add(value.Name, value.SerializedValue?.ToString());
        }
        config.Save(ConfigurationSaveMode.Modified);
    }
}
