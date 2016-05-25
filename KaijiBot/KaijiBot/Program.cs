using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;
using KaijiBot.Settings;
using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Fiddler;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace KaijiBot
{
    class Program
    {

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {

            #region misc
            //Disable fiddler on app exit
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            // Catch all the exceptions
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandedExHandler);
            #endregion

            Config.Load();

            var connector = new Proxy.ProcessConnector();
            var prc = connector.Connect("chrome");
            var ts = new Proxy.GameProxy(prc);
            var gameEventEmmiter = new Game.EventEmitter(ts);
            var table = new Game.Table(gameEventEmmiter);
            Logger.LoggerContoller.MainLogger.Info(
                "Bot started. Press \"Q\" to exit");
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                new LowLevelBullshit.KeyHook();
            }).Start();
            while (true) ;
        }

        

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                FiddlerApplication.Shutdown();
            }
            return false;
        }
        static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        static void UnhandedExHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            Logger.LoggerContoller.MainLogger.Error(ex);
            Logger.LoggerContoller.MainLogger.Info("Press any key to exit");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
