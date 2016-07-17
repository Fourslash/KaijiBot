using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaijiBot.Logger;
using KaijiBot.Game.DecisionMaking;
using System.Drawing;

namespace KaijiBot.Game
{
    enum TableStates
    {
        AwaitingDeal,
        Deal,
        AwaitingDraw,
        Draw,
        AwaitingDoubleStart,
        DoubleStart,
        AwaitingDoubleEnd,
        DoubleEnd,
        AwaitingDoubleRetire,
        DoubleRetire,
        AwaitingCaptcha,
        EnteringCaptcha                   
    }

    class Table
    {

        public TableStates State
        {
            get
            {
                return state_;
            }
            set
            {
                LoggerContoller.GameLogger.Debug(
                    String.Format("Table state changed to {0}",
                    value.ToString()));
                if (Settings.Config.Values.TelegramNotifications == true)
                {
                    Telegram.Messenger.CheckCommands();
                }
                state_ = value;
            }
        }
        public DoubleEnd LastDoubleResult { get; set; }
        private EventEmitter emitter_;
        private TableStates state_;
        private TableClicker tableClicker_;
        private Action<object> lastAction_;
        private object lastParam_;

        public Table(EventEmitter emitter)
        {            
            emitter_ = emitter;
            tableClicker_ = new TableClicker(emitter.Process);
            emitter_.Deal += OnDeal;
            emitter_.DoubleEnd += OnDoubleEnd;
            emitter_.DoubleRetire += OnDoubleRetire;
            emitter_.DoubleStart += OnDoubleStart;
            emitter_.Draw += OnDraw;
            emitter_.Captcha += OnCaptcha;
            emitter_.CaptchaResult += OnCaptchaResult;  
        }

        private void OnCaptchaResult(object result)
        {            
            var captchaResult = (CaptchaResult)result;
            if (captchaResult.IsCorrect == true)
            {
                if (Settings.Config.Values.TelegramNotifications == true)
                {
                    Telegram.Messenger.SendMessage("Success!");
                }
                lastAction_(lastParam_);
            }
        }

        private async void OnCaptcha()
        {
            State = TableStates.AwaitingCaptcha;
            LoggerContoller.GameLogger.Info("Captcha detected.");
            if (Settings.Config.Values.TelegramNotifications == true) {
                System.Threading.Thread.Sleep(1000);
                var bitmap = LowLevelBullshit.WindowFinder.MakeScreenshot(emitter_.Process);
                System.Threading.Thread.Sleep(1000);
                int yCoord = LowLevelBullshit.PixelFinder.GetCaptchaYCoord(bitmap);
                yCoord += 10; //move to center of te bar
                State = TableStates.EnteringCaptcha;
                string captcha = await Telegram.Messenger.SendCapthca();
                tableClicker_.CaptchaClick(yCoord);
                System.Threading.Thread.Sleep(1000);
                LowLevelBullshit.KeyPresser.WriteWord(emitter_.Process, captcha);
                System.Threading.Thread.Sleep(1000);
                tableClicker_.CaptchaButtonClick(yCoord);
            }
        }


        private void OnDraw(object result)
        {
            lastAction_ = OnDraw;
            lastParam_ = result;

            var drawResult = (DrawResult)result;
            State = TableStates.Draw;
            LoggerContoller.GameLogger.Info(
                String.Format("Draw result: {0}", drawResult.HandResult.ToString()));
            tableClicker_.DrawClick(drawResult.IsWin);
            State = drawResult.IsWin ? TableStates.AwaitingDoubleStart
                : TableStates.AwaitingDeal;            
        }

        private void OnDoubleStart(object result)
        {
            lastAction_ = OnDoubleStart;
            lastParam_ = result;

            State = TableStates.DoubleStart;
            var dm = new DoubleDecisionMaker(((DoubleStart)result).FirstCard);
            var BetSide = dm.ChosenBetSide;
            tableClicker_.DoubleStartClick(BetSide);
            State = TableStates.AwaitingDoubleEnd;
        }

        private void OnDoubleRetire(object result)
        {
            lastAction_ = OnDoubleRetire;
            lastParam_ = result;

            State = TableStates.DoubleRetire;
            tableClicker_.RetireClick();            
            State = TableStates.AwaitingDeal;
        }

        private void OnDoubleEnd(object result)
        {
            lastAction_ = OnDoubleEnd;
            lastParam_ = result;

            var doubleEnd = (DoubleEnd)result;
            State = TableStates.DoubleEnd;
            LastDoubleResult = doubleEnd;
            if (doubleEnd.NextGame)
            {
                var dm = new DoubleDecisionMaker(doubleEnd.SecondCard);
                bool playMore = dm.IsWorthPlayMore(doubleEnd.MedalDiff);
                tableClicker_.DoubleEndClick(playMore);
                State = TableStates.AwaitingDoubleStart;
            } else
            {
                DoubleEndLog(doubleEnd.MedalDiff);
                tableClicker_.RetireClick();
                State = TableStates.AwaitingDeal;
            }   
        }

        private void OnDeal(object result)
        {
            lastAction_ = OnDeal;
            lastParam_ = result;

            State = TableStates.Deal;
            var dm = new DealDecisionMaker(((DealResult)result).Cards.ToArray());
            var kept = dm.getKeptCards();
            LoggerContoller.GameLogger.Info(
              String.Format("Keeping: {0}", kept.Length.ToString()));
            tableClicker_.DealClick(kept);
            State = TableStates.AwaitingDraw;
        }

        private void DoubleEndLog(int medalDiff)
        {
            LoggerContoller.GameLogger.Info(
                  String.Format("Balance change: {0}", medalDiff.ToString()));
        }
    }
}
