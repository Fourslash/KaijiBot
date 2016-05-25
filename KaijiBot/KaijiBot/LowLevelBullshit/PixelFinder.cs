using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KaijiBot.LowLevelBullshit
{
    class PixelFinder
    {
        const int R = 242;
        const int G = 238;
        const int B = 226;

        public static int GetCaptchaYCoord(Bitmap bitmap)
        {
            Logger.LoggerContoller.MainLogger.Debug("Finding input field on the screenshot");
            int result = 0;
            for (int i = 0; i < bitmap.Height && result == 0 ; i++)
            {
                var pixel = bitmap.GetPixel(187, i);
                if (IsColorOk(pixel.R, R) && IsColorOk(pixel.G, G) && IsColorOk(pixel.B, B))
                {
                    result = i;
                }
            }
            if (result == 0)
            {
                throw new Exception("Cant find Y coord for input field on the bitmap");
            }
            return result;
        }

        private static bool IsColorOk(int expected, int value)
        {
            return Math.Abs(expected - value) <= 3;
        }
    }
}
