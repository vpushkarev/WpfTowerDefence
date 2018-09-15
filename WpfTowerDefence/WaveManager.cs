using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfTowerDefence
{
    class WaveManager
    {
        public List<Enemy> enemies = new List<Enemy>();
        public List<Cell> WayPoints { get; private set; }
        public Canvas CanvasMap { get; private set; }
        public Game XamlGame { get; private set; }
        int creepSpawnTimer = 7000;
        public int waveNumber = 1;
        public int waveCount = 5;
        public List<WaveSpawn> WavesSpawn = new List<WaveSpawn>();
        WaveSpawn waveSpawn;
        private DispatcherTimer timer;

        public WaveManager(List<Cell> wayPoints, Canvas canvasMap, Game xamlGame)
        {
            WayPoints = wayPoints;
            CanvasMap = canvasMap;
            XamlGame = xamlGame;
        }

        public void StartWaveSpawner()
        {
            timerWaveStart();
        }

        public void GenerateWave(Object sender, EventArgs e)
        {
            if (waveNumber == waveCount)
            {
                timer.Stop();
            }
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
              waveSpawn = new WaveSpawn(WayPoints, CanvasMap, waveNumber, waveNumber);
                WavesSpawn.Add(waveSpawn);
                waveSpawn.onCount += EnemySpawn_onCount;
            }));

           waveSpawn.GenerateEnemiesWave();
            waveNumber++;
        }

        private void EnemySpawn_onCount(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        private void timerWaveStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(GenerateWave);
            timer.Interval = new TimeSpan(0, 0, 0, 0, creepSpawnTimer);
            timer.Start();
        }

        public void StopWaveSpawner()
        {
            timer.Stop();
        }

    }
}
