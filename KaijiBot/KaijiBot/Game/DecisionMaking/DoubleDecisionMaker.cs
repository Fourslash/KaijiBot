using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KaijiBot.Game.DecisionMaking
{
    enum BetSides { High, Low }

    class DoubleDecisionMaker
    {
        private static Ranks[] allRanks = getDoubleRanks();
        private Card FirstCard { get; set; }
        public BetSides ChosenBetSide { get; set; }
        public int WinChance { get; set; }


        public DoubleDecisionMaker (Card firstCard)
        {
            FirstCard = firstCard;
            CalculateWinChanse();
        }

        public bool IsWorthPlayMore(int bet)
        {            
            Logger.LoggerContoller.GameLogger.Verbose(WinChance.ToString());
            if (WinChance >= 91)
                return true;
            else if (WinChance >= 83)
                return bet < 50000;
            else if (WinChance >= 75)
                return bet < 30000;
            else if (WinChance >= 66)
                return bet < 15000;
            else if (WinChance >= 58)
                return bet < 8500;
            else if (WinChance >= 50)
                return bet < 4000;
            else if (WinChance >= 41)
                return bet < 2000;
            else
                throw new Exception("Unreachable region");
        }

        private void CalculateWinChanse()
        {
            var higherRanks = allRanks.Where(x => FirstCard.Rank < x);
            var lowerRanks = allRanks.Where(x => FirstCard.Rank > x);
            int targetRankCount;
            if (higherRanks.Count() > lowerRanks.Count())
            {
                targetRankCount = higherRanks.Count();
                ChosenBetSide = BetSides.High;
            } else if (higherRanks.Count() < lowerRanks.Count())
            {
                targetRankCount = lowerRanks.Count();
                ChosenBetSide = BetSides.Low;
            } else
            {
                Random rand = new Random();
                if (rand.Next(0, 2) == 0)
                {
                    targetRankCount = higherRanks.Count();
                    ChosenBetSide = BetSides.High;
                }
                else {
                    targetRankCount = lowerRanks.Count();
                    ChosenBetSide = BetSides.Low;
                }
            }
            WinChance = targetRankCount * 100 / allRanks.Count() - 1;
        }

        

        private static Ranks[] getDoubleRanks()
        {
            var list = Enum.GetValues(typeof(Ranks))
                .Cast<Ranks>().ToList();
            list.Remove(Ranks.Joker);
            return list.ToArray();
        }


    }
}
