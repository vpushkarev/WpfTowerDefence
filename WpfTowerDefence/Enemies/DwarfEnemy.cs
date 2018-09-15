using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence.Enemies
{
    class DwarfEnemy : Enemy
    {
        static SolidColorBrush colorNormal = Brushes.Peru;
        static int startHealth = 12;
        static int killCost = 5;

        public DwarfEnemy(List<Cell> wayPoints, Canvas canvasMap) : base(wayPoints, canvasMap, colorNormal, startHealth, killCost)
        {
        }
    }
}
