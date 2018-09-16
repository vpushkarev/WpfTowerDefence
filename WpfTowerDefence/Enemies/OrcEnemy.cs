using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTowerDefence.Enemies
{
    class OrcEnemy : Enemy
    {
        static SolidColorBrush colorNormal = Brushes.PapayaWhip;
        static int startHealth = 4;
        static int killCost = 3;

        public OrcEnemy(List<Cell> wayPoints, Canvas canvasMap) : base (wayPoints, canvasMap, colorNormal, startHealth, killCost)
        {
        }
    }
}
