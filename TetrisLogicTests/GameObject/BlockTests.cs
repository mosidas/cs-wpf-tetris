using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.Tests
{
    [TestClass()]
    public class BlockTests
    {
        [TestMethod()]
        public void BlockTest_IBlock()
        {
            var block = new Block(BlockTypes.I);
            Assert.AreEqual(BlockTypes.I, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(-1, block.Location.Y);
            var expect =
@"0,0,0,0
1,1,1,1
0,0,0,0
0,0,0,0";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_TBlock()
        {
            var block = new Block(BlockTypes.T);
            Assert.AreEqual(BlockTypes.T, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(0, block.Location.Y);
            var expect =
@"0,1,0
1,1,1
0,0,0";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_LBlock()
        {
            var block = new Block(BlockTypes.L);
            Assert.AreEqual(BlockTypes.L, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(0, block.Location.Y);
            var expect =
@"0,0,1
1,1,1
0,0,0";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_JBlock()
        {
            var block = new Block(BlockTypes.J);
            Assert.AreEqual(BlockTypes.J, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(0, block.Location.Y);
            var expect =
@"1,0,0
1,1,1
0,0,0";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_SBlock()
        {
            var block = new Block(BlockTypes.S);
            Assert.AreEqual(BlockTypes.S, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(0, block.Location.Y);
            var expect =
@"0,1,1
1,1,0
0,0,0";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_ZBlock()
        {
            var block = new Block(BlockTypes.Z);
            Assert.AreEqual(BlockTypes.Z, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(0, block.Location.Y);
            var expect =
@"1,1,0
0,1,1
0,0,0";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_OBlock()
        {
            var block = new Block(BlockTypes.O);
            Assert.AreEqual(BlockTypes.O, block.BlockType);
            Assert.AreEqual(3, block.Location.X);
            Assert.AreEqual(0, block.Location.Y);
            var expect =
@"1,1
1,1";
            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_MoveLoation()
        {
            var block = new Block(BlockTypes.O);
            var before = block.Location;
            var x = 1;
            var y = 2;
            block.MoveLocation(x,y);
            var after = block.Location;

            Assert.AreEqual(before.X + x, after.X);
            Assert.AreEqual(before.Y + y, after.Y);
        }

        [TestMethod()]
        public void BlockTest_ResetLoation()
        {
            var block = new Block(BlockTypes.O);
            var before = block.Location;
            block.MoveLocation(2, 1);
            block.ResetLocation();
            var after = block.Location;

            Assert.AreEqual(before.X, after.X);
            Assert.AreEqual(before.Y, after.Y);
        }

        [TestMethod()]
        public void BlockTest_RotateRight()
        {
            var block = new Block(BlockTypes.T);
            block.RotateRight();

            var expect =
@"0,1,0
0,1,1
0,1,0";

            Assert.AreEqual(expect, block.DrawBlock());
        }

        [TestMethod()]
        public void BlockTest_RotateLeft()
        {
            var block = new Block(BlockTypes.T);
            block.RotateLeft();

            var expect =
@"0,1,0
1,1,0
0,1,0";

            Assert.AreEqual(expect, block.DrawBlock());
        }
    }
}