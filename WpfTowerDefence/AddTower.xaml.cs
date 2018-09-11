using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для AddTower.xaml
    /// </summary>
    public partial class AddTower : Window
    {
        Cell cell;
        Canvas canvasMap;
        internal Tower tower;
        TowerManager towerManager;

        internal AddTower( Cell cell, TowerManager towerManager, Canvas canvasMap)
        {
            this.cell = cell;
            this.canvasMap = canvasMap;
            this.towerManager = towerManager;
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AddNewTower( cell/*, towerManager, canvasMap*/);
            this.Close();
        }

        private void AddNewTower(Cell cell/*, TowerManager towerManager, Canvas canvasMap*/)
        {
            if (towerManager != null && cell.state != 1 && cell.state != 2)
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    tower = new Tower(cell, canvasMap);
                    towerManager.AddTower(tower);
                }));
                cell.state = 2;
            }
        }
   
    }
}
