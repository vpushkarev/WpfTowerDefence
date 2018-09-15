using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence.Enemies
{
    class AssassinEnemy : Enemy
    {
        static SolidColorBrush colorNormal = Brushes.PeachPuff;
        static int startHealth = 6;
        static int killCost = 4;
        public AssassinEnemy(List<Cell> wayPoints, Canvas canvasMap) : base (wayPoints, canvasMap, colorNormal, startHealth, killCost)
        {
        }
    }
}
