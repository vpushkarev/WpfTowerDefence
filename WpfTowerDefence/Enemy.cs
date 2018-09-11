using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    class Enemy : UserControl
    {
        public SolidColorBrush colorNorm { get; } = Brushes.Fuchsia;
        public SolidColorBrush colorHit { get; } = Brushes.Firebrick;
        protected int startHealth = 4;
        protected int killCost = 1;
        public int CurrentHealth { get; private set; }
        public bool IsKilled => CurrentHealth <= 0;
        List<Cell> wayPoints = new List<Cell>();
        public Cell CurrWayPoint { get; private set; }
        public int WayIndex { get; private set; } = 0; //Step number
        //double tmpWayIndex = 0; //use for speed
        //float speed = 1.0f;

        Canvas canvasMap;
        public Label EnemyLable { get; private set; }


        public Enemy(List<Cell> wayPoints, Canvas canvasMap)
        {
            this.wayPoints = wayPoints;
            canvasMap = canvasMap;
            EnemyLable = CreateEnemyLable(new Point(wayPoints[WayIndex].X, wayPoints[WayIndex].Y), colorNorm);
            canvasMap.Children.Add(EnemyLable);
            CurrentHealth = startHealth;
        }

        private Label CreateEnemyLable(Point point, Brush brush)
        {
            Label cell = new Label();
            cell.Width = 45;
            cell.Height = 45;
            cell.Background = brush;
            cell.Visibility = Visibility.Hidden;
            Canvas.SetLeft(cell, point.X);
            Canvas.SetTop(cell, point.Y);
            return cell;
        }

        public void Update(Game xamlGame, Timer timer)
        {
            Move(xamlGame, timer);
        }

        private void Move(Game xamlGame, Timer timer)
        {
            if (WayIndex == wayPoints.Count - 1)
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    //timer.Dispose();
                    xamlGame.resultGame="Вы проиграли!";
                    xamlGame.Close();
                    //You Louser!
                }));
            }
            else
            {
                if (EnemyLable.Visibility == Visibility.Hidden)
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        EnemyLable.Visibility = Visibility.Visible;
                    }));
                }
                else
                {
                    WayIndex++;
                    //tmpWayIndex = tmpWayIndex + speed;
                    //WayIndex = (int)Math.Floor(tmpWayIndex);

                    CurrWayPoint = wayPoints[WayIndex];

                    int left = (int)CurrWayPoint.X;
                    int top = (int)CurrWayPoint.Y;

                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Canvas.SetLeft(EnemyLable, left);
                        Canvas.SetTop(EnemyLable, top);
                    }));
                }
            }
        }

        public void DecreaseHealth(int power)
        {
            CurrentHealth -= power;
        }

    }
}
