using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaijiBot.Logger;
using KaijiBot.Game.DecisionMaking;
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
        DoubleRetire        
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
                state_ = value;
            }
        }
        public DoubleEnd LastDoubleResult { get; set; }
        private EventEmitter emitter_;
        private TableStates state_;
        private TableClicker tableClicker_;

        public Table(EventEmitter emitter)
        {            
            emitter_ = emitter;
            tableClicker_ = new TableClicker(emitter.Process);
            emitter_.Deal += OnDeal;
            emitter_.DoubleEnd += OnDoubleEnd;
            emitter_.DoubleRetire += OnDoubleRetire;
            emitter_.DoubleStart += OnDoubleStart;
            emitter_.Draw += OnDraw;            
        }

        private void OnDraw(DrawResult result)
        {
            State = TableStates.Draw;
            LoggerContoller.GameLogger.Info(
                String.Format("Draw result: {0}", result.HandResult.ToString()));
            tableClicker_.DrawClick(result.IsWin);
            State = result.IsWin ? TableStates.AwaitingDoubleStart
                : TableStates.AwaitingDeal;            
        }

        private void OnDoubleStart(DoubleStart result)
        {
            State = TableStates.DoubleStart;
            var dm = new DoubleDecisionMaker(result.FirstCard);
            var BetSide = dm.ChosenBetSide;
            tableClicker_.DoubleStartClick(BetSide);
            State = TableStates.AwaitingDoubleEnd;
        }

        private void OnDoubleRetire(DoubleRetire result)
        {
            State = TableStates.DoubleRetire;
            tableClicker_.RetireClick();            
            State = TableStates.AwaitingDeal;
        }

        private void OnDoubleEnd(DoubleEnd result)
        {
            State = TableStates.DoubleEnd;
            LastDoubleResult = result;
            if (result.NextGame)
            {
                var dm = new DoubleDecisionMaker(result.SecondCard);
                bool playMore = dm.IsWorthPlayMore(result.MedalDiff);
                tableClicker_.DoubleEndClick(playMore);
                State = TableStates.AwaitingDoubleStart;
            } else
            {
                DoubleEndLog(result.MedalDiff);
                tableClicker_.RetireClick();
                State = TableStates.AwaitingDeal;
            }   
        }

        private void OnDeal(DealResult result)
        {
            State = TableStates.Deal;
            var dm = new DealDecisionMaker(result.Cards.ToArray());
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
