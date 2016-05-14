using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using KaijiBot.Logger;

namespace KaijiBot.Settings
{
    class Config
    {
        const string CONFIG_PATH = @"config.json";
        public static SettingsRecord Values { get { return values_; } }
        static SettingsRecord values_;

        public static void Load()
        {
            try {
                var json = File.ReadAllText(CONFIG_PATH);
                values_ = JsonConvert.DeserializeObject<SettingsRecord>(json);
                validateValues();
            } catch (FileNotFoundException ex)
            {
                LoggerContoller.MainLogger.Warn("Cant find \"config.json\". Creating default config file.");
                createDefaultConfig();                
            }
        }

        private static void createDefaultConfig()
        {
            values_ = new SettingsRecord();
            var json = JsonConvert.SerializeObject(values_, Formatting.Indented);
            File.WriteAllText(CONFIG_PATH, json);
        }

        private static void validateValues()
        {
            if (values_.TelegramNotifications == true)
            {
                if (values_.TelegramTargetUserId == null || values_.TelegramApiKey == null)
                {
                    values_.TelegramNotifications = false;
                    LoggerContoller.MainLogger.Warn("Telegram config is not filled properly. Disabling telegram notifications.");
                }
            }
        }
    }

    public class SettingsRecord
    {
        public bool TelegramNotifications { get; set; } = false;
        public string TelegramApiKey { get; set; } = null;
        public string TelegramTargetUserId { get; set; } = null;
    };

}
