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
    enum ApiEvents { Retire, Draw, Deal }

    class EventEmitter
    {

        public delegate void DER(DealResult result);
        public event DER Deal;

        public delegate void DR (DrawResult result);
        public event DR Draw;

        private GameProxy proxy_;
        public EventEmitter(GameProxy proxy)
        {
            proxy_ = proxy;
            proxy.NewDataCollected += ProcessData;
        }


        private void ProcessData(string jsonString, string url)
        {            
            var data = DynamicJson.Parse(jsonString);
            LoggerContoller.GameLogger.Debug(url);
            string type = getEventType(url);


            LoggerContoller.GameLogger.Debug(type);
            switch (type)
            {
                case "poker_draw":
                    ProcessDraw(jsonString);
                    break;
                case "poker_deal":
                    ProcessDeal(jsonString);
                    break;
                /*case "poker_double_retire":
                    return ApiEvents.Retire;*/
                default:
                    // throw new Exception(String.Format("Unknown api event: {0}", type));
                    LoggerContoller.GameLogger.Error(type);
                    break;
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

        private string getEventType(string url)
        {
            var type = url.Split('/')[2];
            return type.Remove(type.IndexOf('?'));
        } 
    }
}
