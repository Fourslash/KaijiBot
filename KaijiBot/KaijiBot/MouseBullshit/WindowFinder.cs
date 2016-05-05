using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace KaijiBot.MouseBullshit
{
    class WindowFinder
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static Rect GetWindow(System.Diagnostics.Process process)        
        {
            IntPtr ptr = process.MainWindowHandle;
            Rect rect = new Rect();
            GetWindowRect(ptr, ref rect);
            return rect;
        }
    }
}
