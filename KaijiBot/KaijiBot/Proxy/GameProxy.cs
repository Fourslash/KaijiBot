using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using System.IO;

namespace KaijiBot.Proxy
{
    class GameProxy
    {
        private int ProcessID {get; set;}
        public delegate void UI(string jsonString, string apiString);
        public static event UI NewDataCollected;

        public GameProxy(int processId)
        {
            this.ProcessID = processId;
            FiddlerStart();

        }
        public void Dispose()
        {
            //LogWriter.WriteLog("Shutting down Fiddler");
            Fiddler.FiddlerApplication.Shutdown();
        }
        void FiddlerStart()
        {
            try
            {

                #region settings

                FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);
                FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;
                //FiddlerCoreStartupFlags.
                Fiddler.FiddlerApplication.Startup(0, oFCSF);
                #endregion

                Fiddler.FiddlerApplication.BeforeRequest += BeforeRequest;
                Fiddler.FiddlerApplication.BeforeResponse += BeforeResponse;
                //LogWriter.WriteLogSucces("Fiddler started");
            }
            catch (Exception ex)
            {
                //LogWriter.WriteLogOnException(ex);
                throw ex;
            }

        }

        void BeforeRequest(Session oSession)
        {
            /*if (oSession.LocalProcess.ToLower().Contains("skypebot2"))
            {
                oSession.Ignore();
            }*/
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
            if (oS.LocalProcessID != this.ProcessID)
            {
                return false;
            } else if (!oS.oResponse.MIMEType.Equals("application/json"))
            {
                return false;
            } else if (!oS.PathAndQuery.Contains("/casino_poker"))
            {
                return false;
            } else
            {
                return true;
            }

        }

        void ProcessSession(Session oS)
        {
            oS.utilDecodeResponse();

            using (Stream receiveStream = new MemoryStream(oS.ResponseBody))
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    //Console.WriteLine(string.Format("Processing {0}", oS.PathAndQuery));
                    string tmp = readStream.ReadToEnd();
                    Logger.LoggerContoller.ProxyLogger.Debug(tmp);
                    //string jsonStr = readStream.ReadToEnd().Remove(0, 7);
                    //string apiStr = oS.PathAndQuery;
                    //var json = DynamicJson.Parse(str);
                    //NewDataCollected(jsonStr, apiStr);

                }
            }
        }
    }
}
