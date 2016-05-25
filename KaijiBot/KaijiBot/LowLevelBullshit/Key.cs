using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.LowLevelBullshit
{

    class Key
    {
        public int Value { get; set; }
        public char Letter { get; set; }

        public static Key getKey(char key)
        {
            Key result = null;
            int? value = null;
            switch (key)
            {
                #region codes
                case '0': value = 0x30; break;
                case '1': value = 0x31; break;
                case '2': value = 0x32; break;
                case '3': value = 0x33; break;
                case '4': value = 0x34; break;
                case '5': value = 0x35; break;
                case '6': value = 0x36; break;
                case '7': value = 0x37; break;
                case '8': value = 0x38; break;
                case '9': value = 0x39; break;

                case 'a': value = 0x41; break;
                case 'b': value = 0x42; break;
                case 'c': value = 0x43; break;
                case 'd': value = 0x44; break;
                case 'e': value = 0x45; break;
                case 'f': value = 0x46; break;
                case 'g': value = 0x47; break;
                case 'h': value = 0x48; break;
                case 'i': value = 0x49; break;
                case 'j': value = 0x4A; break;
                case 'k': value = 0x4B; break;
                case 'l': value = 0x4C; break;
                case 'm': value = 0x4D; break;
                case 'n': value = 0x4E; break;
                case 'o': value = 0x4F; break;
                case 'p': value = 0x50; break;
                case 'q': value = 0x51; break;
                case 'r': value = 0x52; break;
                case 's': value = 0x53; break;
                case 't': value = 0x54; break;
                case 'u': value = 0x55; break;
                case 'v': value = 0x56; break;
                case 'w': value = 0x57; break;
                case 'x': value = 0x58; break;
                case 'y': value = 0x59; break;
                case 'z': value = 0x5A; break;
                    #endregion
            }
            if (value != null)
            {
                result = new Key
                {
                    Letter = key,
                    Value = (int)value
                };
            }
            return result;
        }
    }
}
