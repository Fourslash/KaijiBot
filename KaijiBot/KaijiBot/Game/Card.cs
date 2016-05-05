using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
    enum Suits { Spade = 1, Heart = 2, Diamond = 3, Club = 4, Joker = 99 }
    enum Ranks { _2 =2, _3 = 3, _4 = 4, _5 =5,  _6 = 6, _7 = 7, _8 = 8, _9 = 9, _10 = 10, Jack = 11, Queen = 12, King = 13, Ace = 14, Joker = 99}
    class Card
    {
        public Suits Suit { get; set; }
        public Ranks Rank { get; set; }
        public Card (string raw)
        {
            Suit = getSuit(raw);
            Rank = getRank(raw);
        }

        private Ranks getRank(string raw)
        {
            Int32 rank = Convert.ToInt32(raw.Split('_')[1]);
            rank = rank == 1 ? 14 : rank; // fix bullshit
            return (Ranks)rank;
        }

        private Suits getSuit(string raw)
        {
            string suit = raw.Split('_')[0];
            return (Suits)Convert.ToInt32(suit);
        }

        public static bool isEqualSuit(Card a, Card b)
        {
            return a.Suit == Suits.Joker ||
                b.Suit == Suits.Joker ||
                a.Suit == b.Suit;
        }

        public static bool isEqualRank(Card a, Card b)
        {
            return a.Rank == Ranks.Joker ||
                b.Rank == Ranks.Joker ||
                a.Rank== b.Rank;
        }
        

    }
}
