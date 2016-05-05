using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
    class DoubleStart
    {
        protected dynamic data;
        public int Medal { get; set; }
        public Card FirstCard { get; set; }
        public DoubleStart(string json)
        {
            data = Codeplex.Data.DynamicJson.Parse(json);
            FirstCard = new Card(data.card_first);
            Medal = Convert.ToInt32(data.medal.number);
        }
    }
}
