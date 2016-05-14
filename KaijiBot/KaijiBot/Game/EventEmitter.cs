using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaijiBot.Proxy;
using KaijiBot.Logger;
using Codeplex.Data;

namespace KaijiBot.Game
{

    class EventEmitter
    {

        public delegate void DER(DealResult result);
        public event DER Deal;

        public delegate void DR (DrawResult result);
        public event DR Draw;

        public delegate void DBS(DoubleStart result);
        public event DBS DoubleStart;

        public delegate void DBE(DoubleEnd result);
        public event DBE DoubleEnd;

        public delegate void DBR(DoubleRetire result);
        public event DBR DoubleRetire;

        public event Action Captcha;

        private GameProxy proxy_;

        public System.Diagnostics.Process Process
        {
            get
            {
                return proxy_.GameProcess;
            }
        }

        public EventEmitter(GameProxy proxy)
        {
            proxy_ = proxy;
            proxy.NewDataCollected += ProcessData;
        }


        private void ProcessData(string jsonString, string url)
        {            
            var data = DynamicJson.Parse(jsonString);
            LoggerContoller.GameLogger.Verbose(url);
            string type = getEventType(url);


            LoggerContoller.GameLogger.Debug(String.Format("current action: {0}", type));
            switch (type)
            {
                case "poker_draw":
                    ProcessDraw(jsonString);
                    break;
                case "poker_deal":
                    ProcessDeal(jsonString);
                    break;
                case "poker_double_retire":
                    ProcessDoubleRetire(jsonString);
                    break;
                case "poker_double_start":
                    ProcessDoubleStart(jsonString);
                    break;
                case "poker_double_result":
                    ProcessDoubleEnd(jsonString);
                    break;
                case "image":
                    Captcha();
                    break;
                default:
                    throw new Exception(String.Format("Unknown api event: {0}", type));
            }
        }

        private void ProcessDraw(string json)
        {
            var drawResult = new DrawResult(json);
            if (Draw != null)
            {
                Draw(drawResult);
            }
        }

        private void ProcessDeal(string json)
        {
            var dealResult = new DealResult(json);
            if (Deal != null)
            {
                Deal(dealResult);
            }
        }

        private void ProcessDoubleStart(string json)
        {
            var start = new DoubleStart(json);
            if (DoubleStart != null)
            {
                DoubleStart(start);
            }
        }

        private void ProcessDoubleEnd(string json)
        {
            var end = new DoubleEnd(json);
            if (DoubleEnd != null)
            {
                DoubleEnd(end);
            }
        }

        private void ProcessDoubleRetire(string json)
        {
            var retire = new DoubleRetire(json);
            if (DoubleRetire != null)
            {
                DoubleRetire(retire);
            }
        }

        private string getEventType(string url)
        {
            var type = url.Split('/')[2];
            if (type.IndexOf('?') != -1) 
                type = type.Remove(type.IndexOf('?'));
            return type;
        } 
    }
}
