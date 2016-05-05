using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
    class DealResult
    {
        protected dynamic data;
        public int Medal { get; set; }
        public List<Card> Cards { get; set; }
        public DealResult(string json)
        {
            data = Codeplex.Data.DynamicJson.Parse(json);
            Cards = new List<Card>();
            foreach (KeyValuePair<string, dynamic> item in data.card_list)
            {
                Cards.Add(new Card(item.Value));
            }
            Medal = Convert.ToInt32(data.medal.number);
        }
    }
}
