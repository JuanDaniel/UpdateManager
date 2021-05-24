using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace BBI.JD
{
    public static class Config
    {
        static string APPDATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string CFGFOLDER_PATH = Path.Combine(APPDATA_PATH, "BBI UpdateManager");
        static string CFGFILE_PATH = Path.Combine(CFGFOLDER_PATH, "UpdateManager.config");

        public static string Get(string key)
        {
            Configuration config = GetConfiguration();

            if (config != null)
            {
                KeyValueConfigurationElement element = config.AppSettings.Settings[key];

                if (element != null)
                {
                    return element.Value;
                }
            }

            return string.Empty;
        }

        public static void Set(string key, string value)
        {
            Configuration config = GetConfiguration();

            if (config != null)
            {
                KeyValueConfigurationElement element = config.AppSettings.Settings[key];

                if (element != null)
                {
                    element.Value = value;

                    config.Save(ConfigurationSaveMode.Modified);
                }
            }
        }

        public static UpdateDaemonCollection GetAddinsUpdate()
        {
            Configuration config = GetConfiguration();

            if (config != null)
            {
                UpdateDaemon section = (UpdateDaemon)config.GetSection("updateDaemon");

                if (section != null)
                {
                    return section.AddinsUpdate;
                }
            }

            return new UpdateDaemonCollection();
        }

        public static void SetAddinsUpdate(List<Addin> addins)
        {
            Configuration config = GetConfiguration();

            if (config != null)
            {
                UpdateDaemon section = (UpdateDaemon)config.GetSection("updateDaemon");

                if (section != null)
                {
                    UpdateDaemonCollection addinsUpdate = new UpdateDaemonCollection();

                    foreach (var addin in addins)
                    {
                        UpdateDaemonElement element = new UpdateDaemonElement();
                        element.Id = addin.Id;
                        element.Name = addin.Name;
                        element.Version = addin.Version;
                        element.File = addin.Install;

                        addinsUpdate.Add(element);
                    }

                    section.AddinsUpdate = addinsUpdate;

                    config.Save(ConfigurationSaveMode.Modified);
                }
            }
        }

        public static bool AddinHasUpdate(string key, string value)
        {
            Configuration config = GetConfiguration();

            if (config != null)
            {
                UpdateDaemon section = (UpdateDaemon)config.GetSection("updateDaemon");

                if (section != null)
                {
                    UpdateDaemonCollection addinsUpdate = section.AddinsUpdate;

                    foreach (UpdateDaemonElement addin in addinsUpdate)
                    {
                        foreach (var prop in typeof(UpdateDaemonElement).GetProperties())
                        {
                            if (prop.Name == key)
                            {
                               if ((string)prop.GetValue(addin) == value)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static Configuration GetConfiguration()
        {
            Configuration config = null;

            try
            {
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = CFGFILE_PATH;

                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            }
            catch (Exception ex) { }

            return config;
        }
    }

    public class UpdateDaemonElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("version")]
        public string Version
        {
            get { return (string)this["version"]; }
            set { this["version"] = value; }
        }
        [ConfigurationProperty("file")]
        public string File
        {
            get { return (string)this["file"]; }
            set { this["file"] = value; }
        }
    }

    [ConfigurationCollection(typeof(UpdateDaemonElement), AddItemName = "update")]
    public class UpdateDaemonCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UpdateDaemonElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UpdateDaemonElement)element).Id;
        }

        public void Add(UpdateDaemonElement element)
        {
            BaseAdd(element);
        }
    }

    public class UpdateDaemon : ConfigurationSection
    {
        [ConfigurationProperty("addinsUpdate", IsDefaultCollection = true)]
        public UpdateDaemonCollection AddinsUpdate
        {
            get { return (UpdateDaemonCollection)this["addinsUpdate"]; }
            set { this["addinsUpdate"] = value; }
        }
    }
}
