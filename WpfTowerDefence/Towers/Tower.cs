using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    public class Tower : Canvas
    {
        int kRange = 50;
        protected int range;
        protected int power;
        protected SolidColorBrush ColorNorm { get; private set; }
        protected SolidColorBrush ColorShoot { get; } = Brushes.Red;
        public Cell Location { get; private set; }
        public int UpgradeLevel { get; private set; } = 1;
        public int maxUpgradeLevel = 3;
        public int upgradeCost => UpgradeLevel * 3;
        public int Price { get; private set; }
        public Enemy Target { get; private set; }

        Player player;
        Canvas CanvasMap;
        Button towerLable;
        public bool isSingleUpgrade = false;
        public delegate void MethodContainer(int killCost);
        public event MethodContainer onCount;

        public Tower(Cell location, Canvas canvasMap, SolidColorBrush colorNorm, int price, Player player)
        {
            ColorNorm = colorNorm;
            Location = location;
            CanvasMap = canvasMap;
            Price = price;
            this.player = player;
            towerLable = CreateTowerLable(location, ColorNorm);
            CanvasMap.Children.Add(towerLable);
        }

        private Button CreateTowerLable(Cell location, Brush brush)
        {
            Button tower = new Button();
            tower.Width = 45;
            tower.Height = 45;
            tower.Background = brush;
            Canvas.SetLeft(tower, location.X);
            Canvas.SetTop(tower, location.Y);
            tower.Click += Button_Click;
            return tower;
        }

        /************************************************/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isSingleUpgrade)
            {
                isSingleUpgrade = true;
                UpgradeTower upgradeTower = new UpgradeTower(this, player);
                upgradeTower.Show();
            }
        }
        /************************************************/

        public void FindTarget(List<Enemy> enemies)
        {
            Dictionary<double, Enemy> suitableEnemies = new Dictionary<double, Enemy>();

            Target = null;
            if (Target == null || !Location.InRangeOf(Target.CurrWayPoint, range * kRange))
            {
                double smallestDistance = range * kRange;
                foreach (var enemy in enemies)
                {
                    if (enemy.CurrWayPoint != null && Location.DistanceTo(enemy.CurrWayPoint) < smallestDistance)
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

                        target.EnemyLable.Background = target.ColorHit;
                        towerLable.Background = ColorNorm;
                        await Task.Delay(200);

                        target.EnemyLable.Background = target.ColorNormal;

                        if (target.IsKilled)
                        {
                            CanvasMap.Children.Remove(target.EnemyLable);
                            onCount?.Invoke(target.KillCost);
                            Debug.WriteLine("Враг убит!!! x={0} y={1}", target.CurrWayPoint.X, target.CurrWayPoint.Y);
                        }
                    }));
                }
            }
        }

        public void Upgrade()
        {
            this.UpgradeLevel++;
            this.power = this.power * (this.UpgradeLevel);
            this.range = this.range + (this.UpgradeLevel * 2);
        }

    }
}
