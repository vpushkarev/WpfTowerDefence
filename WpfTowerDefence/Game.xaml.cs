using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfTowerDefence
{
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        Window startWindow;
        WaveManager waveManager;
        TowerManager towerManager;
        Player player;
        public string resultGame;

        string[] path =
            {
            "010111000",
            "010101000",
            "010101000",
            "010101111",
            "010100000",
            "011100000"
        };

        Cell firstCell;
        static int fieldHeight = 6, fieldWidth = 9;
        Cell[,] allCells = new Cell[fieldHeight, fieldWidth];
        List<Cell> wayPoints = new List<Cell>();
        int currWayX, currWayY;
        DispatcherTimer timer;
        int EnemyInterval = 1000;
        public bool isSingleAddTower = false;

        Thread playerThread;
        Thread towersThread;

        public Game(Window startWindow)
        {
            this.startWindow = startWindow;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateLevel();
            wayPoints = LoadWaypoints(allCells, firstCell, fieldWidth, fieldHeight, currWayX, currWayY);
            player = new Player();
            waveManager = new WaveManager(wayPoints, CanvasMap, this);
            towerManager = new TowerManager(waveManager, waveManager.WavesSpawn);

            MoneyLabel.Content = player.Money;
            player.OnCount += ChangedMoney_onCount;

            playerThread = new Thread(() =>
            {
                player.TimerPlayerStart();
            });
            playerThread.Start();

            towersThread = new Thread(() =>
            {
                towerManager.ActivateTowers();
            });
            towersThread.Start();

            waveManager.StartWaveSpawner();
            timerEnemiesStart();
        }

        private void ChangedMoney_onCount(double money)
        {
            Action action = () => { MoneyLabel.Content = money; };
            Application.Current.Dispatcher.Invoke(action);
        }

        private void timerEnemiesStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(MoveEnemeis);
            timer.Interval = new TimeSpan(0, 0, 0, 0, EnemyInterval);
            timer.Start();
        }

        public void MoveEnemeis(Object obj, EventArgs e)
        {
            if (waveManager.waveNumber == waveManager.waveCount + 1 && waveManager.enemies.Count == 0)
            {
                this.resultGame = "Вы победили!";
                this.Close();
            }
            else
            {
                for (int i = 0; i < waveManager.enemies.Count; i++)
                {
                    waveManager.enemies[i].Update(this, timer);
                }
            }
        }

        private void CreateLevel()
        {
            for (int i = 0; i < fieldHeight; i++)
                for (int k = 0; k < fieldWidth; k++)
                {
                    int groundIndex = int.Parse(path[i].ToCharArray()[k].ToString());
                    var color = (groundIndex == 0) ? Brushes.Brown : Brushes.Gray;
                    CreateCell(color, groundIndex, k, i);
                }
        }

        private void CreateCell(SolidColorBrush color, int GroundIndex, int x, int y)
        {
            int x0 = 10, y0 = 10;
            Point cellPos = new Point(x0 + (x * 50), y0 + (y * 50));

            Cell cell = new Cell((int)cellPos.X, (int)cellPos.Y);
            Button cellButton = CreateButton(cellPos, color, GroundIndex, cell);
            CanvasMap.Children.Insert(x + y, cellButton);
            cell.SetState(GroundIndex);

            if (!cell.isGround && firstCell == null)
            {
                firstCell = cell;
                currWayX = x;
                currWayY = y;
            }
            allCells[y, x] = cell;
        }

        private Button CreateButton(Point point, Brush brush, int GroundIndex, Cell cell)
        {
            Button button = new Button();
            button.Width = 45;
            button.Height = 45;
            button.Background = brush;
            Canvas.SetLeft(button, point.X);
            Canvas.SetTop(button, point.Y);
            if (GroundIndex == 1)
            {
                //button.IsEnabled = false;
            }
            else
            {
                button.Tag = cell;
                button.Click += Button_Click;
            }
            return button;
        }

        /************************************************/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isSingleAddTower)
            {
                var button = sender as Button;
                var tag = button.Tag as Cell;
                isSingleAddTower = true;
                AddTower addTower = new AddTower(tag, towerManager, CanvasMap, player, this);
                addTower.Show();
            }
        }
        /************************************************/

        public List<Cell> LoadWaypoints(Cell[,] allCells, Cell firstCell, int fieldWidth, int fieldHeight, int currWayX, int currWayY)
        {
            List<Cell> wayPoints = new List<Cell>();
            Cell currWayGo;
            wayPoints.Add(firstCell);

            while (true)
            {
                currWayGo = null;

                int[] x = new int[4] { -1, 1, 0, 0 };
                int[] y = new int[4] { 0, 0, -1, 1 };
                bool wayFound = false;
                for (int i = 0; i < x.Length; ++i)
                {
                    int offsetX = currWayX + x[i];
                    int offsetY = currWayY + y[i];

                    if (offsetX < 0 || offsetX >= fieldWidth || offsetY < 0 || offsetY >= fieldHeight)
                    {
                        continue;
                    }

                    if (!allCells[offsetY, offsetX].isGround && !wayPoints.Exists(t => t == allCells[offsetY, offsetX]))
                    {
                        currWayGo = allCells[offsetY, offsetX];
                        currWayX += x[i];
                        currWayY += y[i];
                        Debug.WriteLine($"Move ({x[i]}, {y[i]}).");
                        wayFound = true;

                        break;
                    }
                }

                if (!wayFound)
                {
                    break;
                }   

                wayPoints.Add(currWayGo);
            }
            return wayPoints;
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            startWindow.Visibility = Visibility.Visible;
            MainWindow.main.Result = resultGame;
            this.Close();

            waveManager.StopWaveSpawner();
            towerManager.DeactivateTowers();
            player.TimerPlayerStop();
        }

    }
}

