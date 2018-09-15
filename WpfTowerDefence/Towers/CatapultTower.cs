using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    class CatapultTower : Tower
    {
        static SolidColorBrush colorNorm = Brushes.RoyalBlue;
        public static int price = 15;
        public CatapultTower(Cell location, Canvas canvasMap, Player player) : base(location, canvasMap, colorNorm, price, player)
        {
            this.range = 2;
            this.power = 2;
        }
    }
}
