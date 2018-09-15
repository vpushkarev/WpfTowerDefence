using System.ComponentModel;

namespace WpfTowerDefence.Enums
{
    public enum TowerType
    {
        [Description("Лучник     Стоимость: 10")]
        ArcherTower,
        [Description("Катапульта Стоимость: 15")]
        CatapultTower,
        [Description("Требушет   Стоимость: 20")]
        TrebuchetTower
    }
}
