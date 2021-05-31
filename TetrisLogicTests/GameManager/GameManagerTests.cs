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
            Assert.AreEqual(manager.IsGameOver, true);
            Assert.AreEqual(manager.GameLevel, gl);
            Assert.AreEqual(manager.FixedBlockPoints.Count, 0);
            Assert.AreEqual(manager.CurrentBlockPoints.Count, 4);
            Assert.AreEqual(manager.CurrentBlockPoints.Count, 4);
        }

        [TestMethod()]
        public void UpdateTest_CurrentBlocklocation()
        {
            var manager = new GameManager(new Field(), new BlocksPoolManagerDummy());
            manager.Start();
            var before = manager.CurrentBlockPoints;
            manager.Update(ActionTypes.nothing, true);
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
            manager.Start();
            var before = manager.CurrentBlockPoints;
            for (var i = 0; i < 10; i++)
            {
                manager.Update(ActionTypes.nothing, true);
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
            manager.Start();
            var before = manager.CurrentBlockPoints;
            for (var i = 0; i < 19; i++)
            {
                manager.Update(ActionTypes.nothing, true);
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
            manager.Start();
            for (var i = 0; i < 19; i++)
            {
                manager.Update(ActionTypes.nothing, true);
            }
            var ret = manager.FixedBlockPoints;

            Assert.AreEqual(ret.Count,4);
            //for (var i = 0; i < ret.Count; i++)
            //{
            //    Assert.AreEqual(ret[i].X, 18);
            //    Assert.AreEqual(ret[i].X, 18);
            //}
        }
    }
}