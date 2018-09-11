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
    class WaveManager
    {
        public List<Cell> WayPoints { get; private set; }
        public Canvas CanvasMap { get; private set; }
        public Game XamlGame { get; private set; }

        int creepSpawnTimer = 7000;
        public int waveNumber = 1;
        public List<WaveSpawn> WavesSpawn = new List<WaveSpawn>();

        Timer timer;
        WaveSpawn enemySpawn;

        System.Windows.Threading.DispatcherTimer DT;

        public WaveManager(List<Cell> wayPoints, Canvas canvasMap, Game xamlGame)
        {
            WayPoints = wayPoints;
            CanvasMap = canvasMap;
            XamlGame = xamlGame;
        }

        public void StartWaveSpawner()
        {
            timerStart();

            //timer = new Timer(GenerateWave, null, 0, creepSpawnTimer);
            //GenerateWave(new object(), null);
        }

        public void GenerateWave(Object sender, EventArgs e)
        {
            //await Task.Delay(7000);
            if (waveNumber == 5)
            {
                //timer.Change(Timeout.Infinite, Timeout.Infinite);
                //timer.Dispose();
            }
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
              enemySpawn = new WaveSpawn(WayPoints, CanvasMap, waveNumber, waveNumber, XamlGame, timer);
                WavesSpawn.Add(enemySpawn);
            }));

          enemySpawn.GenerateEnemiesWave();
            waveNumber++;
        }

        public void StopWaveSpawner()
        {
            //timer.Change(Timeout.Infinite, Timeout.Infinite);
            //Thread.Sleep(2000);
            //timer.Dispose();
        }

        private void timerStart()
        {
            DT = new System.Windows.Threading.DispatcherTimer();// System.Windows.Threading.DispatcherPriority.Render);  
            DT.Tick += new EventHandler(GenerateWave);
            DT.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            DT.Start();
        }

    }
}
