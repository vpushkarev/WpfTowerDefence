using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTowerDefence
{
    class Player
    {
        private int money = 50;

        private List<Tower> towers = new List<Tower>();

        private int cellX;
        private int cellY;
        private string newTowerType;
        private int tileX;
        private int tileY;
        //private Level level;
        //private ContentManager content;
        protected Tower selectedTower;

        //protected UpgradeButton upgradeButton;
        protected bool buttonVisible;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public string NewTowerType
        {
            set { newTowerType = value; }
        }

        private Canvas canvasMap;


        public Player(Canvas canvasMap)
        {
            this.canvasMap = canvasMap;
            /*this.level = level;
            this.content = content;
            upgradeButton = new UpgradeButton();*/
        }

        //public void Update(GameTime gameTime, List<Enemy> enemies)
        //{
        //    var mouseState = Mouse.GetPosition(canvasMap); //GetState();

        //    cellX = (int)(mouseState.X / 50);
        //    cellY = (int)(mouseState.Y / 50);

        //    tileX = cellX * 50;
        //    tileY = cellY * 50;

        //    if (mouseState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
        //    {

        //        if (string.IsNullOrEmpty(newTowerType) == false)
        //        {
        //            AddTower();
        //        }
        //        else
        //        {
        //            if (selectedTower != null)
        //            {
        //                if (!selectedTower.Bounds.Contains(mouseState.X, mouseState.Y))
        //                {
        //                    selectedTower.Selected = false;
        //                    if (buttonVisible)
        //                        buttonVisible = false;
        //                }
        //            }

        //            foreach (Tower tower in towers)
        //            {
        //                //if (tower==selectedTower)
        //                //{
        //                //  continue;
        //                //}

        //                if (tower.Bounds.Contains(mouseState.X, mouseState.Y))
        //                {
        //                    selectedTower = tower;
        //                    tower.Selected = true;
        //                    buttonVisible = true;
        //                    upgradeButton = new UpgradeButton(upgradeTexture, new Vector2(64, level.Height * 50));
        //                    if (money >= 10 && tower.UpgradeLevel <= 2)
        //                    {
        //                        buttonVisible = true;
        //                        //upgradeButton = new UpgradeButton(upgradeTexture, new Vector2(64, level.Height * 32-32));
        //                        upgradeButton = new UpgradeButton(upgradeTexture, new Vector2(tileX + 5, tileY - 24));

        //                    }
        //                }
        //            }
        //        }
        //    }

        //    foreach (Tower tower in towers)
        //    {
        //        if (tower.HasTarget == false)
        //        {
        //            tower.GetClosestEnemy(enemies);
        //        }

        //        tower.Update(gameTime);
        //    }
        //    oldState = mouseState;

        //    upgradeButton.Update();

        //    if (upgradeButton.Clicked)
        //    {
        //        upgradeButton.Clicked = false;
        //        buttonVisible = false;
        //        //ArrowTower at = (ArrowTower)selectedTower;
        //        selectedTower.Upgrade(selectedTower.UpgradeLevel);
        //        selectedTower.UpgradeLevel += 1;
        //        money -= 10;
        //    }
        //    // Game1.endText = String.Format("Score : {0}",money);
        }



        


        //public void AddTower()
        //{
        //    Tower towerToAdd = null;

        //    switch (newTowerType)
        //    {
        //        case "Arrow Tower":
        //            {
        //                towerToAdd = new ArrowTower(towerTextures[0], bulletTexture, new Vector2(tileX, tileY), content);
        //                break;
        //            }

        //        case "Spike Tower":
        //            {
        //                towerToAdd = new SpikeTower(towerTextures[1], bulletTexture, new Vector2(tileX, tileY), content);
        //                break;
        //            }
        //    }
        //    if (IsCellClear() == true && towerToAdd.Cost <= money)
        //    {
        //        towers.Add(towerToAdd);
        //        money -= towerToAdd.Cost;
        //        newTowerType = string.Empty;
        //    }
        //}
    //}
}
