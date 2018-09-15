using System.Diagnostics;
using System.Windows;

namespace WpfTowerDefence
{
    /// <summary>
    /// Логика взаимодействия для UpgradeTower.xaml
    /// </summary>
    public partial class UpgradeTower : Window
    {
        Tower tower;
        Player player;
        public UpgradeTower(Tower tower, Player player)
        {
            InitializeComponent();
            this.tower = tower;
            this.player = player;
            if (tower.UpgradeLevel == tower.maxUpgradeLevel)
            {
                upgradeTextBox.Text = "Достигнут максимальный уровень союзника";
                upgradeButton.IsEnabled = (bool)false;
            }
            else
            {
                upgradeTextBox.Text = string.Format("Уровень союзника={0} Стоимость улучшения={1}", tower.UpgradeLevel, tower.upgradeCost);
            }
        }

        private void upgradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money >= tower.upgradeCost)
            {
                player.AddMoney(-tower.upgradeCost);
                tower.Upgrade();
                Debug.WriteLine("Upgrade done.");
                this.Close();
            }
            else
            {
                upgradeTextBox.Text = "Недостаточно денег для повышения уровня этого союзника.";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tower.isSingleUpgrade = false;
        }
    }
}
