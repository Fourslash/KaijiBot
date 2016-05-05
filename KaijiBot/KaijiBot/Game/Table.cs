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
        public DoubleEnd LastDoubleResult { get; set; }las

        public Table(EventEmitter emitter)
        {
            emitter_ = emitter;
            emitter_.Deal += OnDeal;
            emitter_.DoubleEnd += OnDoubleEnd;
            emitter_.DoubleRetire += OnDoubleRetire;
            emitter_.DoubleStart += OnDoubleStart;
            emitter_.Draw += OnDraw;            
        }

        private void OnDraw(DrawResult result)
        {
            State = TableStates.Draw;
        }

        private void OnDoubleStart(DoubleStart result)
        {
            State = TableStates.DoubleStart;
            var dm = new DoubleDecisionMaker(result.FirstCard);
            var t = dm.IsWorthPlayMore(1337);
        }

        private void OnDoubleRetire(DoubleRetire result)
        {
            State = TableStates.DoubleRetire;
        }

        private void OnDoubleEnd(DoubleEnd result)
        {
            State = TableStates.DoubleEnd;
            LastDoubleResult = 
        }

        private void OnDeal(DealResult result)
        {
            State = TableStates.Deal;
        }

        private EventEmitter emitter_;
        private TableStates state_;
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
    }
}
