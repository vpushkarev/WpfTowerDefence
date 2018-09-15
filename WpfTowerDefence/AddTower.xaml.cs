using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTowerDefence
{
    /// <summary>
    /// Логика взаимодействия для AddTower.xaml
    /// </summary>
    public partial class AddTower : Window
    {
        Cell cell;
        Canvas canvasMap;
        TowerManager towerManager;
        Player player;
        Game game;

        internal AddTower(Cell cell, TowerManager towerManager, Canvas canvasMap, Player player, Game game)
        {
            this.cell = cell;
            this.canvasMap = canvasMap;
            this.towerManager = towerManager;
            this.player = player;
            this.game = game;
            InitializeComponent();
        }

        private void buyButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewTower();
            if (errorTextBox.Text.Equals(""))
            {
                this.Close();
            }
        }

        private void AddNewTower()
        {
            Tower towerToAdd = null;
            if (towerType_listBox.SelectedItem == null)
            {
                return;
            }

            var selectedTowerType = towerType_listBox.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(selectedTowerType) && towerManager != null && cell.isGround)
            {
                switch (selectedTowerType)
                {
                    case "ArcherTower":
                        {
                            towerToAdd = new ArcherTower(cell, canvasMap, player);
                            break;
                        }
                    case "CatapultTower":
                        {
                            towerToAdd = new CatapultTower(cell, canvasMap, player);
                            break;
                        }
                    case "TrebuchetTower":
                        {
                            towerToAdd = new TrebuchetTower(cell, canvasMap, player);
                            break;
                        }
                }

                if(player.Money < towerToAdd.Price)
                { 
                    errorTextBox.Text = "Недостаточно денег для покупки этой башни.";
                }
                else
                {
                    errorTextBox.Text = "";
                    player.AddMoney(-towerToAdd.Price);
                    towerToAdd.onCount += KillCost_onCount;
                    //Application.Current.Dispatcher.Invoke((Action)(() =>
                    //{
                        towerManager.AddTower(towerToAdd);
                    //}));
                    cell.state = 2;
                }
            }
        }

        private void KillCost_onCount(int killCost)
        {
            player.AddMoney(killCost);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            game.isSingleAddTower = false;
        }
    }
}
