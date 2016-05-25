using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Game
{
   
    class CaptchaResult
    {
        private dynamic data;
        public bool IsCorrect { get; set; }
        public CaptchaResult(string json) 
        {
            data = Codeplex.Data.DynamicJson.Parse(json);
            IsCorrect = Convert.ToBoolean(data.is_correct);
        }
    }
}
