using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfTowerDefence
{
    public class Enemy
    {
        public SolidColorBrush ColorNormal { get; private set; }
        public SolidColorBrush ColorHit { get; } = Brushes.Firebrick;
        protected int StartHealth { get; private set; }
        public int KillCost { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsKilled => CurrentHealth <= 0;
        List<Cell> wayPoints;
        public Cell CurrWayPoint { get; private set; }
        public int WayIndex { get; private set; } = 0; //Step number
        //double tmpWayIndex = 0;
        //float speed = 1.0f;
        public Label EnemyLable { get; private set; }

        public Enemy(List<Cell> wayPoints, Canvas canvasMap, SolidColorBrush colorNorm, int startHealth, int killCost)
        {
            StartHealth = startHealth;
            ColorNormal = colorNorm;
            this.wayPoints = wayPoints;
            EnemyLable = CreateEnemyLable(new Point(wayPoints[WayIndex].X, wayPoints[WayIndex].Y), colorNorm);
            canvasMap.Children.Add(EnemyLable);
            CurrentHealth = startHealth;
            KillCost = killCost;
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

        public void Update(Game windowGame, DispatcherTimer timer)
        {
            Move(windowGame, timer);
        }

        private void Move(Game windowGame, DispatcherTimer timer)
        {
            if (WayIndex == wayPoints.Count - 1)
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    timer.Stop();
                    windowGame.resultGame = "Вы проиграли!";
                    windowGame.Close();
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
                    //tmpWayIndex += speed;
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
