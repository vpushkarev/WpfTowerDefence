using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfTowerDefence
{
    class TowerManager : Canvas
    {
        WaveManager waveManager;
        Canvas canvasMap;
        Cell[,] allCells;
        List<WaveSpawn> wavesSpawn = new List<WaveSpawn>();
        List<Tower> towers = new List<Tower>();
        Timer timer;
        int towerFireSpeed = 1000;

        public TowerManager(WaveManager waveManager, Canvas canvasMap, Cell[,] allCells, List<WaveSpawn> wavesSpawn)
        {
            this.waveManager = waveManager;
            this.canvasMap = canvasMap;
            this.allCells = allCells;
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
            timer = new Timer(FireOnEnemies, towers, 0, towerFireSpeed);
        }

        public void FireOnEnemies(Object obj)
        {
            List<Tower> towers = (List<Tower>)obj;
            foreach (var tower in towers)
            {
                if (wavesSpawn != null)
                {
                    tower.FindTarget(wavesSpawn);
                    tower.FireByTarget(tower);
                }

                if (tower.Target != null && tower.Target.IsKilled)
                {
                    var wave = wavesSpawn.FirstOrDefault(k => k.enemies.Contains(tower.Target));
                    if (wave != null)
                    {
                        wavesSpawn[wave.waveNumber - 1].enemies.Remove(tower.Target);
                    }
                }
            }
        }

        public void DeactivateTowers()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            //Thread.Sleep(2000);
            //timer.Dispose();
        }

    }
}
