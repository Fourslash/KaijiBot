using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
    class DoubleRetire
    {
        protected dynamic data;
        public int Medal { get; set; }
        public int MedalWon { get; set; }
        public DoubleRetire(string json)
        {
            data = Codeplex.Data.DynamicJson.Parse(json);
            Medal = Convert.ToInt32(data.status.medal.number);
            MedalWon = Convert.ToInt32(data.status.get_medal);
        }
    }
}
