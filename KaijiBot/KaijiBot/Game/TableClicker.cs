using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KaijiBot.LowLevelBullshit;
using KaijiBot.Game.DecisionMaking;

namespace KaijiBot.Game
{
    class Point
    {
        public Point (int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }

    class TableClicker
    {

        const int BUTTON_Y = 575;
        const int CARD_Y = 380;
        const int CARD_TIMEOUT = 400;
        const int DRAW_TIMEOUT = 3100;
        const int CAPTCHA_TIMEOUT = 1000;
        const int CAPTCHA_X = 205;
        const int CAPTCHA_BUTTON_X = 384;

        private int CardTimeout
        {
            get { return randomTimeout(CARD_TIMEOUT, 50); }
        }

        private int DrawTimeout
        {
            get { return randomTimeout(DRAW_TIMEOUT, 150); }
        }

        private int CaptchaTimeout
        {
            get { return randomTimeout(CAPTCHA_TIMEOUT, 150); }
        }

        private Process process;
        public TableClicker(Process proc)
        {
            process = proc;
        }

        public void DrawClick(bool isWin)
        {
            System.Threading.Thread.Sleep(DrawTimeout);
            if (isWin)
                ClickRight();
            else
                ClickMiddle();
        }

        public void DoubleStartClick(DecisionMaking.BetSides side)
        {
            System.Threading.Thread.Sleep(DrawTimeout);
            if (side == DecisionMaking.BetSides.High)
                ClickLeft();
            else
                ClickRight();
        }

        public void RetireClick()
        {
            System.Threading.Thread.Sleep(DrawTimeout);
            ClickMiddle();
        }

        public void DoubleEndClick(bool playMore)
        {
            System.Threading.Thread.Sleep(DrawTimeout);
            if (playMore)
                ClickRight();
            else
                ClickLeft();
        }

        public void DealClick(int[] kept)
        {
            System.Threading.Thread.Sleep(DrawTimeout);
            foreach (var index in kept)
            {
                ClickCard(index);
                System.Threading.Thread.Sleep(CardTimeout);
            }
            ClickMiddle();
        }

        public void CaptchaClick(int yCoord)
        {
            System.Threading.Thread.Sleep(CaptchaTimeout);
            click(randomPoint(new Point(CAPTCHA_X, yCoord), 100, 3));
        }

        public void CaptchaButtonClick(int yCoord)
        {
            System.Threading.Thread.Sleep(CaptchaTimeout);            
            click(randomPoint(new Point(CAPTCHA_BUTTON_X, yCoord), 15, 2));
        }

        public void ClickCard(int num)
        {
            int x;
            switch (num)
            {
                case 0:
                    x = 85; break;
                case 1:
                    x = 170; break;
                case 2:
                    x = 250; break;
                case 3:
                    x = 330; break;
                case 4:
                    x = 415; break;
                default:
                    throw new IndexOutOfRangeException();
            }

            click(randomPoint(new Point(x, CARD_Y), 20));
        }

        private Point randomPoint(Point pt, int xOffset, int? yOffset = null)
        {
            if (yOffset == null)
                yOffset = xOffset;

            Random rand = new Random();
            var modifiedX = rand.Next(xOffset * -1, xOffset + 1) + pt.X;
            var modifiedY = rand.Next((int)yOffset * -1, (int)yOffset + 1) + pt.Y;
            return new Point(modifiedX, modifiedY);
        }

        private int randomTimeout(int time, int offset)
        {
            Random rand = new Random();
            return time + rand.Next(offset * -1, offset + 1);
        }

        private void ClickMiddle()
        {
            click(randomPoint(new Point(257, BUTTON_Y), 25, 10));
        }

        private void ClickLeft()
        {
            click(randomPoint(new Point(178, BUTTON_Y), 25, 10));
        }

        private void ClickRight()
        {
            click(randomPoint(new Point(335, BUTTON_Y), 25, 10));
        }

        private void click(Point p)
        {
            var wPoint = getWindowPoint();
            var adjustedX = p.X + wPoint.X;
            var adjustedY = p.Y + wPoint.Y;
            MouseClicker.LeftMouseClick(adjustedX, adjustedY);
        }

        private Point getWindowPoint()
        {
            var rect = WindowFinder.GetWindow(process);
            return new Point(rect.Left, rect.Top);
        }
    }
}
