using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KaijiBot.MouseBullshit;
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

        const int BUTTON_Y = 590;
        const int CARD_Y = 380;
        private Process process;
        public TableClicker(Process proc)
        {
            process = proc;
        }

        public void DrawClick(bool isWin)
        {
            System.Threading.Thread.Sleep(3000);
            if (isWin)
                ClickRight();
            else
                ClickMiddle();
        }

        public void DoubleStartClick(DecisionMaking.BetSides side)
        {
            System.Threading.Thread.Sleep(3000);
            if (side == DecisionMaking.BetSides.High)
                ClickLeft();
            else
                ClickRight();
        }

        public void RetireClick()
        {
            System.Threading.Thread.Sleep(3000);
            ClickMiddle();
        }

        public void DoubleEndClick(bool playMore)
        {
            System.Threading.Thread.Sleep(3000);
            if (playMore)
                ClickRight();
            else
                ClickLeft();
        }

        public void DealClick(int[] kept)
        {
            System.Threading.Thread.Sleep(3000);
            foreach (var index in kept)
            {
                ClickCard(index);
                System.Threading.Thread.Sleep(500);
            }
            ClickMiddle();
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

            click(x, CARD_Y);
        }

        public void ClickMiddle()
        {
            click(257, BUTTON_Y);
        }

        public void ClickLeft()
        {
            click(178, BUTTON_Y);
        }

        public void ClickRight()
        {
            click(335, BUTTON_Y);
        }

        private void click(int x, int y)
        {
            var wPoint = getWindowPoint();
            var adjustedX = x + wPoint.X;
            var adjustedY = y + wPoint.Y;
            MouseClicker.LeftMouseClick(adjustedX, adjustedY);
        }

        private Point getWindowPoint()
        {
            var rect = WindowFinder.GetWindow(process);
            return new Point(rect.Left, rect.Top);
        }
    }
}
