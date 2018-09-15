using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace WpfTowerDefence
{
    public class Cell : UserControl
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int state; /*0 - Ground; 1- Path; 2 - Tower*/
        public bool isGround => state == 0;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void SetState(int i)
        {
            state = i;
        }

        public bool InRangeOf(Cell location, int range)
        {
            Debug.WriteLine("Distance {0}", DistanceTo(location));
            return DistanceTo(location) <= range;
        }

        public double DistanceTo(Cell cell)
        {
            return DistanceTo(cell.X, cell.Y);
        }

        public double DistanceTo(int x, int y)
        {
            var dist = Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
            return dist;
        }
    }
}
