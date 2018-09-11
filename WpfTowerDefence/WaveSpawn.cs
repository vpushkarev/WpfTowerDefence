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
    class WaveSpawn : Canvas
    {
        public int waveNumber;
        public List<Cell> WayPoints { get; private set; }
        public Canvas CanvasMap { get; private set; }
        public int EnemyCount { get; private set; }
        Game xamlGame;

        int CreepInterval = 1000;
        public List<Enemy> enemies = new List<Enemy>();
        Timer timer;
        Enemy enemy;

        System.Windows.Threading.DispatcherTimer DT;

        public WaveSpawn(List<Cell> wayPoints, Canvas canvasMap, int enemyCount, int waveNumber, Game xamlGame, Timer timer)
        {
            WayPoints = wayPoints;
            CanvasMap = canvasMap;
            EnemyCount = enemyCount;
            this.waveNumber = waveNumber;
            this.xamlGame = xamlGame;
            this.timer = timer;
        }

        public async void GenerateEnemiesWave()
        {
            Timer timerMoveEnemies = new Timer(MoveEnemeis, null, Timeout.Infinite, Timeout.Infinite);

                for (int i = 0; i < EnemyCount; i++)
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    enemy = new Enemy(WayPoints, CanvasMap);
                    enemies.Add(enemy);
                    timerMoveEnemies.Change(0, Timeout.Infinite);
                }));
                await Task.Delay(1000);
            }
            timerMoveEnemies.Change(0, CreepInterval);

            //TODO: Проверить необходимость и работоспособность Dispose
            if (enemies.Count == 0)
            {
                //timerMoveEnemies.Dispose();
            }
        }


        public void MoveEnemeis(Object obj)
        {
            if (waveNumber == 5 && enemies.Count == 0)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                //timer.Dispose();
                xamlGame.resultGame = "Вы победили!";
                xamlGame.Dispatcher.BeginInvoke((Action)(() => { xamlGame.Close(); }));
                //You WIN!
            }
            else
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(xamlGame, timer);
                }
            }
        }
    }
}
