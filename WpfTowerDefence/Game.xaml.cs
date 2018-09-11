using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTowerDefence
{
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public Window startWindow;
        public string resultGame;

        int fieldHeight = 6, fieldWight = 9;
        List<Cell> wayPoints = new List<Cell>();
        Cell firstCell;
        int currWayX, currWayY;

        Cell[,] allCells = new Cell[6, 9];

        string[] path =
    {
        "010111000",
        "010101000",
        "010101000",
        "010101111",
        "010100000",
        "011100000",
    };

        WaveManager waveManager;
        TowerManager towerManager;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        System.Windows.Threading.DispatcherTimer DT;



        public Game(Window startWindow)
        {
            this.startWindow = startWindow;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CancellationToken token = cancellationTokenSource.Token;

            //var task = new Thread(async () => await TestAsync(token));
            //task.Start();

            CreateLevel();
            LoadWaypoints();

            
                //waveManager = new WaveManager(wayPoints, CanvasMap, this);
                // towerManager = new TowerManager(waveManager, CanvasMap, allCells, waveManager.WavesSpawn);


            //Debug.WriteLine("GEnerate Wave:");
            //waveManager.GenerateWave();

           // waveManager.StartWaveSpawner();



            var task = new Thread(async () => await TestAsync(token));
            task.Start();

            //await TestAsync();
        }

       


        private Task TestAsync(CancellationToken token)
        {
            
            //timerStart();
            return Task.Factory.StartNew(() => DT_Tick(new Object(), token));// DT.Start());
        }

        void DT_Tick(object obj, CancellationToken token/*object sender, EventArgs e*/)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                waveManager = new WaveManager(wayPoints, CanvasMap, this);
                // towerManager = new TowerManager(waveManager, CanvasMap, allCells, waveManager.WavesSpawn);
            }));

            //Debug.WriteLine("GEnerate Wave:");
            //waveManager.GenerateWave();

            waveManager.StartWaveSpawner();
            // towerManager.ActivateTowers();
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
            var button = sender as Button;
            var tag = button.Tag as Cell;
            AddTower addTower = new AddTower(tag, towerManager, CanvasMap);
            addTower.Show();
            /*if (addTower.tower != null)
            {
                towerManager.AddTower(addTower.tower);
            }  */
        }

        class AddTowerData
        {
            public Point location;
            public int GroundIndex;
            public Cell cell;
            TowerManager towerManager;
            Canvas canvasMap;
            public AddTowerData(/*Point location, int GroundIndex,*/ Cell cell, TowerManager towerManager, Canvas canvasMap)
            {
                //this.location = location;
                //this.GroundIndex = GroundIndex;
                this.cell = cell;
                this.towerManager = towerManager;
                this.canvasMap = canvasMap;
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
            //towerManager.DeactivateTowers();

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

    }
}

