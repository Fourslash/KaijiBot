using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.IO;
using System.Diagnostics;

namespace KaijiBot.Telegram
{
    class Messenger
    {
        public static void SendMessage(string msg)
        {
            try {
                var bot = new Api(Settings.Config.Values.TelegramApiKey);
                bot.SendTextMessage(Settings.Config.Values.TelegramTargetUserId, msg);
                Logger.LoggerContoller.TelegramLogger.Debug(string.Format("Message \"{0}\" sent to target user", msg));
            } catch(Exception ex)
            {
                Logger.LoggerContoller.TelegramLogger.Error(ex);
            }
        }

        public static async Task<string> SendCapthca()
        {
            await CheckCommands();
            Logger.LoggerContoller.TelegramLogger.Debug("Sending captcha info to Telegram");
            var chatId = (long)Convert.ToDouble(Settings.Config.Values.TelegramTargetUserId);
            var bot = new Api(Settings.Config.Values.TelegramApiKey);
            using (FileStream fsSource = new FileStream("captcha.jpg", FileMode.Open, FileAccess.Read))
            {
                var file = new FileToSend("Captcha.jpg", fsSource);
                await bot.SendPhoto(chatId, file, "Please enter the captcha in double quotes. Example: \"ngm5y\"");
            }
            return await getCaptcha();
        }

        private static void Eject()
        {
            Environment.Exit(0);
        }

        private static void CheckCommand(string command)
        {
            command = command.ToLower().Trim();
            switch (command)
            {
                case "!exit": Eject(); break;
                case "!stop": Eject(); break;
            }
        }

        public static async Task CheckCommands()
        {
            Update[] updates;
            int offset = 0;
            int? oldOffset = null;
            var bot = new Api(Settings.Config.Values.TelegramApiKey);
            var chatId = (long)Convert.ToDouble(Settings.Config.Values.TelegramTargetUserId);
            while (offset != oldOffset)
            {
                updates = await bot.GetUpdates(offset);
                updates = updates.Where(x => x.Message != null &&
                    x.Message.Type == MessageType.TextMessage &&
                    x.Message.Chat.Id == chatId &&                    
                    (DateTime.UtcNow - x.Message.Date).TotalMinutes <= 5).ToArray();
                foreach (Update up in updates)
                {
                    CheckCommand(up.Message.Text);
                    oldOffset = offset;
                    offset = up.Id + 1;
                }
            }
        } 

        private static async Task<string> getCaptcha()
        {            
            Logger.LoggerContoller.TelegramLogger.Debug("Getting captcha answer from Telegram");
            int offset = 0;
            string result = null;
            var bot = new Api(Settings.Config.Values.TelegramApiKey);
            var chatId = (long)Convert.ToDouble(Settings.Config.Values.TelegramTargetUserId);
            while (result == null)
            {                
                var updates = await bot.GetUpdates(offset);
                updates = updates.Where(x => x.Message != null &&
                    x.Message.Type == MessageType.TextMessage &&
                    x.Message.Chat.Id == chatId &&
                    (DateTime.UtcNow - x.Message.Date).TotalMinutes <= 5).ToArray();
                foreach (Update up in updates)
                {
                    
                    if (up.Message.Text.StartsWith("\"") && up.Message.Text.EndsWith("\""))
                    {
                        result = up.Message.Text.Replace("\"", "");
                    }
                    offset = up.Id + 1;
                }
            }
            return result;
        }
    }
}
