using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfTowerDefence.Tests
{
    [TestClass]
    public class gameWindow
    {
        static List<Cell> wayPointsExecuted = new List<Cell>();
        static Cell[,] allCells;
        static Player player;

        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            //map
            //0101
            //0101
            //0111

            allCells = new Cell[3, 4] {
                {new Cell(10,10), new Cell(60,10), new Cell(110,10), new Cell(160,10)},
                {new Cell(10,60), new Cell(60,60), new Cell(110,60), new Cell(160,60)},
                {new Cell(10,110), new Cell(60,110), new Cell(110,110), new Cell(160,110)},
            };

            allCells[0, 0].SetState(0);
            allCells[0, 1].SetState(1);
            allCells[0, 2].SetState(0);
            allCells[0, 3].SetState(1);
            allCells[1, 0].SetState(0);
            allCells[1, 1].SetState(1);
            allCells[1, 2].SetState(0);
            allCells[1, 3].SetState(1);
            allCells[2, 0].SetState(0);
            allCells[2, 1].SetState(1);
            allCells[2, 2].SetState(1);
            allCells[2, 3].SetState(1);

            wayPointsExecuted.Add(allCells[0, 1]);
            wayPointsExecuted.Add(allCells[1, 1]);
            wayPointsExecuted.Add(allCells[2, 1]);
            wayPointsExecuted.Add(allCells[2, 2]);
            wayPointsExecuted.Add(allCells[2, 3]);
            wayPointsExecuted.Add(allCells[1, 3]);
            wayPointsExecuted.Add(allCells[0, 3]);
        }

        [TestMethod]
        public void AddMoneyTest()
        {
            //arrange
            Player player = new Player();
            double input = 10;
            double expected = 20;

            //act
            player.AddMoney(10);
            double actual = player.Money;

            //assert
            Assert.AreEqual(expected, actual, "Money={0}, should have been {1}!", input, expected);
        }

        [TestMethod]
        public void LoadWayPoints()
        {
            //arrange
            Game gameWindow = new Game(null);
            List<Cell> wayPointsActual = new List<Cell>();
            Cell firstCell = allCells[0, 1];
            int currWayX = 1, currWayY = 0;

            //act
            wayPointsActual = gameWindow.LoadWaypoints(allCells, firstCell, 4, 3, currWayX, currWayY);

            //assert
            CollectionAssert.AreEqual(wayPointsExecuted, wayPointsActual);
        }

        [TestMethod]
        public void FireByTarget()
        {
            //arrange
            List<Enemy> enemies = new List<Enemy>();
            Tower tower = new Tower(allCells[2, 0], 1, 3);
            enemies.Add(new Enemy(wayPointsExecuted, 4, 3, allCells[0, 1]));
            enemies.Add(new Enemy(wayPointsExecuted, 5, 3, allCells[0, 1]));

            //act
            tower.Target = enemies[0];
            for (int i = 0; i < 4; i++)
            {
                tower.FireByTarget();
            }
            bool isKilled1 = tower.Target.IsKilled;

            tower.Target = enemies[1];
            for (int i = 0; i < 4; i++)
            {
                tower.FireByTarget();
            }
            bool isKilled2 = tower.Target.IsKilled;

            //assert
            Assert.AreEqual(isKilled1, true);
            Assert.AreEqual(isKilled2, false);
        }

        [TestMethod]
        public void FindTarget()
        {
            //arrange
            List<Enemy> enemies = new List<Enemy>();
            Tower tower = new Tower(allCells[2, 0], 1, 10);
            enemies.Add(new Enemy(wayPointsExecuted, 4, 3, allCells[0, 1]));
            enemies.Add(new Enemy(wayPointsExecuted, 5, 3, allCells[0, 1]));

            //act
            enemies[0].CurrWayPoint = allCells[0, 1];
            enemies[1].CurrWayPoint = allCells[1, 1];
            tower.FindTarget(enemies);
            Enemy target1 = tower.Target;

            enemies[0].CurrWayPoint = allCells[0, 1];
            enemies[1].CurrWayPoint = allCells[0, 3];
            tower.FindTarget(enemies);
            Enemy target2 = tower.Target;

            //assert
            Assert.AreEqual(target1, enemies[1]);
            Assert.AreEqual(target2, enemies[0]);
        }

    }
}
