using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfTowerDefence
{
    class TowerManager : Canvas
    {
        int towerFireSpeed = 1000;
        WaveManager waveManager;
        List<WaveSpawn> wavesSpawn = new List<WaveSpawn>();
        List<Tower> towers = new List<Tower>();
        DispatcherTimer timer;

        public TowerManager(WaveManager waveManager, List<WaveSpawn> wavesSpawn)
        {
            this.waveManager = waveManager;
            this.wavesSpawn = wavesSpawn;
        }

        public void AddTower(Tower tower)
        {
            if (tower != null)
            {
                towers.Add(tower);
            }
        }

        public void ActivateTowers()
        {
            timerTowerStart();
        }

        public void FireOnEnemies(Object obj, EventArgs e)
        {
            foreach (var tower in towers)
            {
                if (wavesSpawn.Count>0)
                {
                    tower.FindTarget(waveManager.enemies);
                    tower.FireByTarget(tower);
                }

                if (tower.Target != null && tower.Target.IsKilled)
                {
                    waveManager.enemies.Remove(tower.Target);
                }
            }
        }

        private void timerTowerStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(FireOnEnemies);
            timer.Interval = new TimeSpan(0, 0, 0, 0, towerFireSpeed);
            timer.Start();
        }

        public void DeactivateTowers()
        {
            timer.Stop();
        }

    }
}
