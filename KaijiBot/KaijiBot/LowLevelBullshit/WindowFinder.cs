using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace KaijiBot.LowLevelBullshit
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

        public static Rect GetWindow(Process process)        
        {
            IntPtr ptr = process.MainWindowHandle;
            Rect rect = new Rect();
            GetWindowRect(ptr, ref rect);
            return rect;
        }

        public static Bitmap MakeScreenshot(Process pr)
        {
            Logger.LoggerContoller.MainLogger.Debug("Taking screenshot");
            var window = LowLevelBullshit.WindowFinder.GetWindow(pr);
            int x = window.Left,
                y = window.Top,
                height = window.Bottom - window.Top,
                width = window.Right - window.Left;

            Rectangle bounds = new Rectangle(x, y, width, height);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }            
            bitmap.Save("captcha.jpg", ImageFormat.Jpeg);
            return bitmap;
        }
    }
}
