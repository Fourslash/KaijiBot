using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace KaijiBot.LowLevelBullshit
{
    class KeyPresser
    {
        const UInt32 WM_KEYDOWN = 0x0100;


        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        public static void WriteWord(Process prc, string word)
        {
            foreach (char ch in word)
            {
                var key = Key.getKey(ch);
                if (key != null)
                    PressKey(prc, key.Value);             
            }

        }

        [STAThread]
        static void PressKey(Process prc, int key)
        {
            PostMessage(prc.MainWindowHandle, WM_KEYDOWN, key, 0);
            Thread.Sleep(1000);
        }
    }
}
