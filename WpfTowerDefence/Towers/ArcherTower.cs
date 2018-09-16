using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    public class ArcherTower : Tower
    {
        static SolidColorBrush colorNorm = Brushes.SkyBlue;
        public const int price = 10; // Make it const or private, or better pass as entity configuration.

        public ArcherTower(Cell location, Canvas canvasMap, Player player) : base(location, canvasMap, colorNorm, price, player)
        {
            this.range = 3;
            this.power = 1;
        }
    }
}
