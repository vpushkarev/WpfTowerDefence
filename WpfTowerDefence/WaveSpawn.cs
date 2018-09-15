using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfTowerDefence.Enemies;
using WpfTowerDefence.Enums;

namespace WpfTowerDefence
{
    class WaveSpawn : Canvas
    {
        public int WaveNumber { get; private set; }
        public List<Cell> WayPoints { get; private set; }
        public Canvas CanvasMap { get; private set; }
        public int EnemyCount { get; private set; }
        Enemy enemyToAdd;
        Array values = Enum.GetNames(typeof(EnemyType));
        Random random = new Random();

        public delegate void MethodContainer(Enemy enemy);
        public event MethodContainer onCount;

        public WaveSpawn(List<Cell> wayPoints, Canvas canvasMap, int enemyCount, int waveNumber)
        {
            WayPoints = wayPoints;
            CanvasMap = canvasMap;
            EnemyCount = enemyCount;
            WaveNumber = waveNumber;
        }

        public async void GenerateEnemiesWave()
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                int randomEnemyType = random.Next(values.Length);
                string randomEnemy = values.GetValue(randomEnemyType).ToString();

                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    switch (randomEnemy)
                    {
                        case "OrkEnemy":
                            {
                                enemyToAdd = new OrkEnemy(WayPoints, CanvasMap);
                                break;
                            }
                        case "AssassinEnemy":
                            {
                                enemyToAdd = new AssassinEnemy(WayPoints, CanvasMap);
                                break;
                            }
                        case "DwarfEnemy":
                            {
                                enemyToAdd = new DwarfEnemy(WayPoints, CanvasMap);
                                break;
                            }
                    }
                }));

                onCount?.Invoke(enemyToAdd);
                await Task.Delay(1000);
            }
        }

    }
}
