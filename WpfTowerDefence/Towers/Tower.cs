using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    class Tower : Canvas
    {
        public SolidColorBrush ColorNorm { get; } = Brushes.BlueViolet;
        public SolidColorBrush ColorShoot { get; } = Brushes.Red;
        int kRange = 50;
        protected int range;
        protected int power;
        //int upgradeLevel = 1;
        protected int price;
        public Enemy Target { get; private set; }

        Canvas CanvasMap;
        Label towerLable;

        public Cell Location { get; private set; }

        public Tower(Cell location, Canvas canvasMap)
        {
            Location = location;
            CanvasMap = canvasMap;
            towerLable = CreateTowerLable(location, ColorNorm);
            CanvasMap.Children.Add(towerLable);
        }

        private Label CreateTowerLable(Cell location, Brush brush)
        {
            Label cell = new Label();
            cell.Width = 45;
            cell.Height = 45;
            cell.Background = brush;
            Canvas.SetLeft(cell, location.X);
            Canvas.SetTop(cell, location.Y);
            return cell;
        }


        public void FindTarget(List<WaveSpawn> enemtSpawns)
        {
            Dictionary<double, Enemy> suitableEnemies = new Dictionary<double, Enemy>();
            List<Enemy> enemies = enemtSpawns.SelectMany(k => k.enemies).ToList();
            //Debug.WriteLine("enemis_count ={0}", enemies.Count);

            Target = null;
            if (Target == null || !Location.InRangeOf(Target.CurrWayPoint, range * kRange))
            {
                double smallestDistance = range * kRange;
                foreach (var enemy in enemies)
                {
                    if (enemy.CurrWayPoint != null && Location.DistanceTo(enemy.CurrWayPoint)< smallestDistance)
                    {
                        smallestDistance = Location.DistanceTo(enemy.CurrWayPoint);
                        Target = enemy;
                    }
                }
            }
        }

        public void FireByTarget(Tower tower)
        {
            var target = tower.Target;
            if (target != null)
            {
                target.DecreaseHealth(power);
                Debug.WriteLine("Попадание! x={0} y={1} health={2}", target.CurrWayPoint.X, target.CurrWayPoint.Y, target.CurrentHealth);


                if (target != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)(async () =>
                    {
                        towerLable.Background = ColorShoot;
                        await Task.Delay(200);

                        target.EnemyLable.Background = target.colorHit;
                        towerLable.Background = ColorNorm;
                        await Task.Delay(200);

                        target.EnemyLable.Background = target.colorNorm;

                        if (target.IsKilled)
                        {
                            CanvasMap.Children.Remove(target.EnemyLable);
                            Debug.WriteLine("Враг убит!!! x={0} y={1}", target.CurrWayPoint.X, target.CurrWayPoint.Y);
                        }
                    }));
                }
            }
        }

        //TODO: Сделать если будет время
        //public void Upgrade()
        //{
        //    this.upgradeLevel++;
        //    this.power = this.power * (this.upgradeLevel);
        //    this.range = this.range + (this.upgradeLevel * 2);

        //}

    }
}
