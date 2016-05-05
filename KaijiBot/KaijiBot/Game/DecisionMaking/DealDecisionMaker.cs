using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KaijiBot.Game.DecisionMaking
{

    class DealDecisionMaker
    {
        private Card[] Cards { get; set; }

        #region Combos
        public bool IsTwoPair
        {
            get
            {
                var parCount = 0;

                //Count pars
                var query = from Card in Cards
                            group Card by Card.Rank;
                foreach (var group in query)
                    if (group.Count() >= 2)
                        parCount++;
                //if there is joker - +1 pair
                if (Cards.Count(x => x.Rank == Ranks.Joker) > 0)
                    parCount++;
                return parCount == 2;
            }
        }

        public bool IsThreeOfAKind
        {
            get
            {
                for (var i = 0; i < 3; i++)
                {
                    if (Cards.Count(x => Card.isEqualRank(x, Cards[i])) >= 3)
                        return true;                   
                }
                return false;
            }
        }

        public bool IsStraight
        {
            get
            {
                bool isStraight = true;
                bool isJokerAvailible =
                    Cards.Any(x => x.Rank == Ranks.Joker);
                var sortedCards = Cards.OrderBy(x => x.Rank);
                for (var i = 0; i < Cards.Length - 1; i++)
                {
                    int rankDiff = 
                        Math.Abs(Cards[i].Rank - Cards[i + 1].Rank);
                    if (rankDiff == 1) { }
                    else if (rankDiff == 2 && isJokerAvailible)
                        isJokerAvailible = false;
                    else
                        isStraight = false;
                }
                return isStraight;
            }
        }

        public bool IsFlush {
            get {
                var isFlush = true;
                for (var i = 0; i < Cards.Length - 1; i++)
                {
                    if (!Card.isEqualSuit(Cards[i], Cards[i + 1]))
                        isFlush = false;
                }
                return isFlush;
            }
        }

        public bool IsFullHouse
        {
            get
            {
                return IsTwoPair && IsThreeOfAKind;
            }
        }

        public bool IsFourOfAKind
        {
            get
            {
                for (var i = 0; i < 2; i++)
                {
                    if (Cards.Count(x => Card.isEqualRank(x, Cards[i])) >= 4)
                        return true;
                }
                return false;
            }
        }

        public bool IsStraightFlush
        {
            get
            {
                return IsFlush && IsStraight;
            }
        }

        public bool IsFiveOfAKind
        {
            get
            {
                if (Cards.Count(x => Card.isEqualRank(x, Cards[0])) >= 5)
                    return true;
                return false;
            }
        }

        public bool IsRoyalStraightFlush
        {
            get
            {
                return IsStraightFlush &&
                    (Cards[0].Rank == Ranks._10 ||
                    Cards[4].Rank == Ranks.Ace);
            }
        }
        #endregion

        public bool IsAnyPair
        {
            get
            {
                return IsTwoPair ||
                    IsThreeOfAKind ||
                    IsStraight ||
                    IsFlush ||
                    IsFullHouse ||
                    IsFourOfAKind ||
                    IsStraightFlush ||
                    IsFourOfAKind ||
                    IsRoyalStraightFlush;
            }
        }

        public int[] getKeptCards()
        {
            var CardList = new List<Card>(Cards);
            var kept = new List<int>();

            // If there is joker - keep it
            var joker = CardList.Find(x => x.Suit == Suits.Joker);
            if (joker != null)
                kept.Add(CardList.IndexOf(joker));

            //Keep all pairs
            var query = from Card in Cards
                        group Card by Card.Rank;
            foreach (var group in query)
                if (group.Count() >= 2)
                    foreach (var card in group)
                        kept.Add(CardList.IndexOf(card));

            //Keep 4 cards with same suit
            var suitQuery = from Card in Cards
                        group Card by Card.Suit;
            foreach (var group in query)
                if (group.Count() >= 4)
                    foreach (var card in group)
                        kept.Add(CardList.IndexOf(card));

            //TODO: add almost straight
            return kept.ToArray();
        }

        public DealDecisionMaker (Card[] cards)
        {
            Cards = cards;
        }


        
    }
}
