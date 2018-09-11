using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTowerDefence.Enums
{
    public enum TowerType
    {
        [Description("Лучник")]
        ArcherTower,
        [Description("Катапульта")]
        CatapultTower,
        [Description("Требушет")]
        TrebuchetTower
    }
}
