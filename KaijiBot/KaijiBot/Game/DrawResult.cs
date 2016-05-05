using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
    enum HandResults {
        RoyalStraitFlush = 1,
        FiveOfAKind = 2,
        StraitFlush = 3,
        FourOfAKind = 4,
        FullHouse = 5,
        Flush = 6,
        Straight = 7,
        ThreeOfAKind = 8,
        TwoPair = 9,
        OnePair = 10,
        NoPairs = 11
    }

    
    class DrawResult : DealResult
    {
        public int MedalDiff { get; set; }
        public bool IsWin { get; set; }
        public HandResults HandResult { get; set; }
        public DrawResult(string json) : base(json) 
        {           
            IsWin = data.result == "lose" ? false : true;
            HandResult = (HandResults)Convert.ToInt32(data.hand_id);
            MedalDiff = Convert.ToInt32(data.pay_medal);
            Medal = Convert.ToInt32(data.medal.number);
        }
    }
}
