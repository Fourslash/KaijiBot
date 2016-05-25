using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using System.IO;
using System.Diagnostics;

namespace KaijiBot.Proxy
{
    class GameProxy : IDisposable
    {
        private int ProcessID {get; set;}
        public Process GameProcess { get; set; }
        public delegate void UI(string jsonString, string apiString);
        public event UI NewDataCollected;

        public GameProxy(Process process)
        {

            ProcessID = process.Id;
            GameProcess = process;
            FiddlerStart();

        }
         
        ~GameProxy()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            Logger.LoggerContoller.ProxyLogger.Info("Shutting down Fiddler");
            Fiddler.FiddlerApplication.Shutdown();
        } 

        void FiddlerStart()
        {
            try
            {

                #region settings
                FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);
                FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;
                Fiddler.FiddlerApplication.Startup(0, oFCSF);
                #endregion

                Fiddler.FiddlerApplication.BeforeRequest += BeforeRequest;
                Fiddler.FiddlerApplication.BeforeResponse += BeforeResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void BeforeRequest(Session oSession)
        {
           if (oSession.LocalProcessID != this.ProcessID)
            {               
                oSession.Ignore();
            }
        }

        void BeforeResponse(Session oS)
        {
            if (IsSessionNeedsProcessed(oS))
            {
                ProcessSession(oS);
            }
        }

        bool IsSessionNeedsProcessed(Session oS)
        {
            bool result = false;            
            if (oS.PathAndQuery.Contains("/casino_poker")) 
                result = true;
            if (oS.PathAndQuery.Contains("/captcha"))
                result = true;
           return result;
        }

        void ProcessSession(Session oS)
        {
            oS.utilDecodeResponse();

            using (Stream receiveStream = new MemoryStream(oS.ResponseBody))
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    string json = readStream.ReadToEnd();
                    if (NewDataCollected != null)
                    {
                        this.NewDataCollected(json, oS.url);
                    }
                }
            }
        }
    }
}
