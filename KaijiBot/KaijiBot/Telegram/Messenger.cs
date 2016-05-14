using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

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
    }
}
