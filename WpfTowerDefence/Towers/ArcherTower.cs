using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfTowerDefence
{
    class ArcherTower : Tower
    {
        public ArcherTower(Cell location, Canvas canvasMap) : base(location, canvasMap)
        {
            range = 3;
            power = 1;
            price = 10;
        }
    }
}
