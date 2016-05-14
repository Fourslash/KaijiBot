using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;
using KaijiBot.Settings;


using System.Diagnostics;


namespace KaijiBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Load();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                new LowLevelBullshit.KeyHook();
            }).Start();
            try {
                var connector = new Proxy.ProcessConnector();
                var prc = connector.Connect("chrome");
                var ts = new Proxy.GameProxy(prc);
                var gameEventEmmiter = new Game.EventEmitter(ts);
                var table = new Game.Table(gameEventEmmiter);
                Logger.LoggerContoller.MainLogger.Info(
                    "Bot started. Press \"Q\" to exit");
                while (true) ;
            } catch (Exception ex)
            {
                Logger.LoggerContoller.MainLogger.Error(ex);
                Logger.LoggerContoller.MainLogger.Info("Press any key to exit");
                Console.ReadKey();
            }
           

        }       
    }
}
