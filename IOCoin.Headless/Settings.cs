using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless
{
    public class Settings
    {
 
        [JsonIgnore]
        private NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public NameValueCollection ReadAllSettings() => appSettings?.Count > 0 ? appSettings : null;

        public string ReadSetting(string key) => appSettings[key] ?? string.Empty;
        public bool AddOrUpdateSettings(string key, string value)
        {
            //try
            //{
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            //}
            //catch (ConfigurationErrorsException)
            //{
            //    return false;
            //}
        }

        [JsonProperty]
        public string daemonPath { get; set; }
        [JsonProperty]
        public string appDataDir { get; set; }
        [JsonProperty]
        public string configFilepath { get; set; }

        [JsonProperty]
        public string rpcUser { get; set; } = "iocoinrpc";
        [JsonProperty]
        public string rpcPassword { get; set; }

        [JsonProperty]
        public string daemonArgBase { get; set; }
        [JsonProperty]
        public string walletPasshrase { get; set; }
        [JsonProperty]
        public string notificationAddress { get; set; }

        

        [JsonProperty]
        public IEnumerable<string> addNodes { get; set; } = new List<string>();

    }
}
