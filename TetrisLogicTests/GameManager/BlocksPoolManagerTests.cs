using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TetrisLogic.Tests
{
    [TestClass()]
    public class BlocksPoolManagerTests
    {
        [TestMethod()]
        public void GetNextBlockTest_7KindsIn1Pool()
        {
            var bpm = new BlocksPoolManager();
            var ret = new List<Block>();
            for(var i = 0;i < 7;i++)
            {
                ret.Add(bpm.TakeNextBlock());
                Assert.AreEqual(i + 1, ret.GroupBy(r => r.BlockType).Count());
            }
        }

        [TestMethod()]
        public void GetNextBlockTest_NotExistsNothingType()
        {
            var bpm = new BlocksPoolManager();
            var ret = new List<Block>();
            for (var i = 0; i < 7; i++)
            {
                ret.Add(bpm.TakeNextBlock());
            }

            Assert.AreEqual(false, ret.Exists(r => r.BlockType == BlockTypes.nothing));
        }

        [TestMethod()]
        public void GetNextBlockTest_SameKindsLoop()
        {
            var bpm = new BlocksPoolManager();
            var ret1 = new List<Block>();
            for (var i = 0; i < 7; i++)
            {
                ret1.Add(bpm.TakeNextBlock());
            }

            var ret2 = new List<Block>();
            for (var i = 0; i < 7; i++)
            {
                ret2.Add(bpm.TakeNextBlock());
            }

            ret1 = ret1.OrderBy(r => r.BlockType).ToList();
            ret2 = ret2.OrderBy(r => r.BlockType).ToList();

            for (var i = 0; i < 7; i++)
            {
                Assert.AreEqual(ret1[i].BlockType, ret2[i].BlockType);
            }


        }

        [TestMethod()]
        public void GetNextBlockTest_10Pools()
        {
            var bpm = new BlocksPoolManager();
            var ret = new List<Block>();
            for (var i = 0; i < 70; i++)
            {
                ret.Add(bpm.TakeNextBlock());
            }

            Assert.AreEqual(7, ret.GroupBy(r => r.BlockType).Count());
        }

        [TestMethod()]
        public void ResetTest()
        {
            var bpm = new BlocksPoolManager();
            _ = bpm.TakeNextBlock();
            bpm.Reset();

            Assert.AreEqual(7, bpm.GetNextBlocksPool().Count());
        }
    }
}