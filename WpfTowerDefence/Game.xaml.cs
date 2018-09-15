using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static int fieldHeight = 6, fieldWight = 9;
        Cell[,] allCells = new Cell[fieldHeight, fieldWight];
        List<Cell> wayPoints = new List<Cell>();
        int currWayX, currWayY;
        DispatcherTimer timer;
        int EnemyInterval = 1000;
        public bool isSingleAddTower = false;

        //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //Thread playerThread;
        //Thread unitsThread;

        public Game(Window startWindow)
        {
            this.startWindow = startWindow;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CancellationToken token = cancellationTokenSource.Token;
            CreateLevel();
            LoadWaypoints();
            player = new Player();
            waveManager = new WaveManager(wayPoints, CanvasMap, this);
            towerManager = new TowerManager(waveManager, waveManager.WavesSpawn);

            //playerThread = new Thread(() =>
            //{
                player.timerPlayerStart();
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    MoneyLabel.Content = player.Money;
                }));
                player.onCount += ChangedMoney_onCount;
            //});
            //playerThread.Start();

            waveManager.StartWaveSpawner();
            towerManager.ActivateTowers();
            timerEnemiesStart();

            //var task = new Thread(async () => await TestAsync(token));
            //task.Start();

            //await TestAsync();
        }

        private void ChangedMoney_onCount(double money)
        {
            MoneyLabel.Content = money; ;
        }

        //private Task TestAsync(CancellationToken token)
        //{
        //    return Task.Factory.StartNew(() => DT_Tick(new Object(), token));
        //}

        //void DT_Tick(object obj, CancellationToken token)
        //{
        //    player.timerPlayerStart();
        //}

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
                for (int k = 0; k < fieldWight; k++)
                {
                    int GroundIndex = int.Parse(path[i].ToCharArray()[k].ToString());
                    var color = (GroundIndex == 0) ? Brushes.Brown : Brushes.Gray;
                    CreateCell(color, GroundIndex, k, i);
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
                // button.IsEnabled = false;
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

        private void LoadWaypoints()
        {
            Cell currWayGo;
            wayPoints.Add(firstCell);

            while (true)
            {
                currWayGo = null;

                if (currWayX > 0 && !allCells[currWayY, currWayX - 1].isGround &&
                    !wayPoints.Exists(x => x == allCells[currWayY, currWayX - 1]))
                {
                    currWayGo = allCells[currWayY, currWayX - 1];
                    currWayX--;
                    Debug.WriteLine("Next Cell is Left");
                }
                else if (currWayX < (fieldWight - 1) && !allCells[currWayY, currWayX + 1].isGround &&
                    !wayPoints.Exists(x => x == allCells[currWayY, currWayX + 1]))
                {
                    currWayGo = allCells[currWayY, currWayX + 1];
                    currWayX++;
                    Debug.WriteLine("Next Cell is Right");
                }
                else if (currWayY > 0 && !allCells[currWayY - 1, currWayX].isGround &&
                    !wayPoints.Exists(x => x == allCells[currWayY - 1, currWayX]))
                {
                    currWayGo = allCells[currWayY - 1, currWayX];
                    currWayY--;
                    Debug.WriteLine("Next Cell is Up");
                }
                else if (currWayY < (fieldHeight - 1) && !allCells[currWayY + 1, currWayX].isGround &&
                    !wayPoints.Exists(x => x == allCells[currWayY + 1, currWayX]))
                {
                    currWayGo = allCells[currWayY + 1, currWayX];
                    currWayY++;
                    Debug.WriteLine("Next Cell is Down");
                }
                else
                {
                    Debug.WriteLine("End way");
                    break;
                }

                wayPoints.Add(currWayGo);
            }
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            startWindow.Visibility = Visibility.Visible;
            MainWindow.main.Result = resultGame;
            this.Close();

            waveManager.StopWaveSpawner();
            towerManager.DeactivateTowers();
            player.timerPlayerStop();

            //cancellationTokenSource.Cancel();
            //cancellationTokenSource.Dispose();
        }

    }
}

