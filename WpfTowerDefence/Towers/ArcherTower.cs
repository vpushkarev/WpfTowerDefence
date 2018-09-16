using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    class ArcherTower : Tower
    {
        static SolidColorBrush colorNorm = Brushes.SkyBlue;
        public const int price = 10;

        public ArcherTower(Cell location, Canvas canvasMap, Player player) : base(location, canvasMap, colorNorm, price, player)
        {
            this.range = 3;
            this.power = 1;
        }
    }
}
