using Microsoft.VisualStudio.TestTools.UnitTesting;
using TetrisLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisLogicTests.Stub;
using TetrisLogic.UserAction;

namespace TetrisLogic.Tests
{
    [TestClass()]
    public class GameManagerTests
    {
        [TestMethod()]
        public void GameManagerTest()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            Assert.AreEqual(manager.IsGameOver, true);
            Assert.AreEqual(manager.FixedBlockPoints.Count, 0);
        }

        [TestMethod()]
        public void StartTest()
        {
            int gl = 5;
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start(gl);
            Assert.AreEqual(false, manager.IsGameOver);
            Assert.AreEqual(gl, manager.GameLevel);
            Assert.AreEqual(0, manager.FixedBlockPoints.Count);
            Assert.AreEqual(4, manager.CurrentBlockPoints.Count);
            Assert.AreEqual(4, manager.CurrentBlockPoints.Count);
        }

        [TestMethod()]
        public void UpdateTest_CurrentBlocklocation()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start(0);
            var before = manager.CurrentBlockPoints;
            manager.Update(ActionTypes.moveDown);
            var after = manager.CurrentBlockPoints;

            for(var i = 0;i < before.Count;i++)
            {
                Assert.AreEqual(before[i].X, after[i].X);
                Assert.AreEqual(before[i].Y + 1, after[i].Y);
            }
        }

        [TestMethod()]
        public void UpdateTest_CurrentBlocklocation_10th()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start(0);
            var before = manager.CurrentBlockPoints;
            for (var i = 0; i < 20; i++)
            {
                manager.Update(i % 2 == 0 ? ActionTypes.moveDown : ActionTypes.nothing);
            }
            var after = manager.CurrentBlockPoints;

            for (var i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i].X, after[i].X);
                Assert.AreEqual(before[i].Y + 10, after[i].Y);
            }
        }

        [TestMethod()]
        public void UpdateTest_CurrentBlocklocation_CurrentBlockWhenSpawnNextBlock()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start(0);
            var before = manager.CurrentBlockPoints;
            for (var i = 0; i < 38; i++)
            {
                manager.Update(i % 2 == 0 ? ActionTypes.moveDown : ActionTypes.nothing);
            }
            var after = manager.CurrentBlockPoints;

            for (var i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i].X, after[i].X);
                Assert.AreEqual(before[i].Y, after[i].Y);
            }
        }

        [TestMethod()]
        public void UpdateTest_CurrentBlocklocation_FixedBlockWhenSpawnNextBlock()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start(0);
            for (var i = 0; i < 38; i++)
            {
                manager.Update(i % 2 == 0 ? ActionTypes.moveDown : ActionTypes.nothing);
            }

            Assert.AreEqual(4, manager.FixedBlockPoints.Count);
        }

        [TestMethod()]
        public void UpdateTest_Hold()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start(0);
            var before = manager.CurrentBlockPoints;
            manager.Update(ActionTypes.nothing);
            manager.Update(ActionTypes.moveDown);
            manager.Update(ActionTypes.hold);

            var after = manager.CurrentBlockPoints;

            for (var i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i].X, after[i].X);
                Assert.AreEqual(before[i].Y, after[i].Y);
            }
        }
    }
}