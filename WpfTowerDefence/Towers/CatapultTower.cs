using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfTowerDefence
{
    class CatapultTower : Tower
    {
        public CatapultTower(Cell location, Canvas canvasMap) : base(location, canvasMap)
        {
            range = 2;
            power = 2;
            price = 20;
        }
    }
}
