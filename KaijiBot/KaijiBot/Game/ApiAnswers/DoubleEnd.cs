using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
    enum DoubleResults { Win, Lose, Tie }

    class DoubleEnd : DoubleStart
    {
        public int MedalDiff { get; set; }
        public int Turn { get; set; }
        public DoubleResults Result { get; set; }
        public bool NextGameFlag { get; set; }
        public Card SecondCard { get; set; }
        public DoubleEnd(string json) : base (json)
        {
            SecondCard = new Card(data.card_second);
            MedalDiff = Convert.ToInt32(data.pay_medal);
            Turn = Convert.ToInt32(data.turn);
            NextGameFlag = data.next_game_flag;
            Result = GetResult(data.result);
        }

        private DoubleResults GetResult(string res)
        {
            if (res == "lose")
            {
                return DoubleResults.Lose;
            } else if (res == "win")
            {
                return DoubleResults.Win;
            } else
            {
                return DoubleResults.Tie;
            }
        }
    }
}
