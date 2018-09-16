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
        public int upgradeCost => UpgradeLevel * 4;
        public int Price { get; private set; }
        public Enemy Target { get; /*private*/ set; }

        Player player;
        Canvas canvasMap;
        Button towerButton;
        public bool isSingleUpgrade = false;
        public delegate void MethodContainer(int killCost);
        public event MethodContainer onCount;

        public Tower(Cell location, Canvas canvasMap, SolidColorBrush colorNorm, int price, Player player)
        {
            ColorNorm = colorNorm;
            Location = location;
            this.canvasMap = canvasMap;
            Price = price;
            this.player = player;
            towerButton = CreateTowerButton(location, ColorNorm);
            canvasMap.Children.Add(towerButton);
        }

        //ctor for Unit test
        public Tower(Cell location, int power, int range)
        {
            Location = location;
            this.power = power;
            this.range = range;
            towerButton = CreateTowerButton(location, Brushes.Red);
        }
        ////////////////////////////////

        private Button CreateTowerButton(Cell location, Brush brush)
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

        public void FireByTarget()
        {
            var target = this.Target;
            if (target != null)
            {
                target.DecreaseHealth(power);
                Debug.WriteLine("Попадание! x={0} y={1} health={2}", target.CurrWayPoint.X, target.CurrWayPoint.Y, target.CurrentHealth);


                if (target != null)
                {
                    Dispatcher.Invoke((Action)(async () =>
                    {
                        towerButton.Background = ColorShoot;
                        await Task.Delay(200);

                        target.EnemyLable.Background = target.ColorHit;
                        towerButton.Background = ColorNorm;
                        await Task.Delay(200);

                        target.EnemyLable.Background = target.ColorNormal;

                        if (target.IsKilled)
                        {
                            canvasMap.Children.Remove(target.EnemyLable);
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
