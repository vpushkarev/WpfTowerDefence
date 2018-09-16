using System;
using System.Collections.Generic;
using System.Threading;
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
        Timer timer;

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

        public void FireOnEnemies(Object obj)
        {
            foreach (var tower in towers)
            {
                if (wavesSpawn.Count>0)
                {
                    tower.FindTarget(waveManager.enemies);
                    tower.FireByTarget();
                }

                if (tower.Target != null && tower.Target.IsKilled)
                {
                    waveManager.enemies.Remove(tower.Target);
                }
            }
        }

        private void timerTowerStart()
        {
            timer = new Timer(FireOnEnemies, null, 0, towerFireSpeed);
        }

        public void DeactivateTowers()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

    }
}
