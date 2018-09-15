using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence
{
    class TrebuchetTower : Tower
    {
        static SolidColorBrush colorNorm = Brushes.DarkSlateBlue;
        public static int price = 20;
        public TrebuchetTower(Cell location, Canvas canvasMap, Player player) : base(location, canvasMap, colorNorm, price, player)
        {
            this.range = 2;
            this.power = 2;
        }
    }
}
