using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UA_MoveDownTests
    {
        [TestMethod()]
        public void ActionTest_LocationYIsPlus1()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();

            // before
            var before = block.Location;

            // do
            act.Action(ref field, ref block, ref holdBlock);

            // after
            var after = block.Location;

            Assert.AreEqual(before.X, after.X);
            Assert.AreEqual(before.Y + 1, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_InitStateCanDown()
        {
            // data
            var block = new Block(BlockTypes.O);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            var ret = act.CanAction(field, block, new Block(BlockTypes.nothing));

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanDown18th()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            for (var i = 0; i < 17; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanNotDown19th()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();

            for (var i = 0; i < 18; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(false, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void CanActionTest_TBlockCanDown18th()
        {
            // data
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            for (var i = 0; i < 17; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void CanActionTest_TBlockCanNotDown19th()
        {
            // data
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();

            for (var i = 0; i < 18; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(false, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void CanActionTest_IBlockCanDown19th()
        {
            // data
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            for (var i = 0; i < 18; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void CanActionTest_IBlockCanNotCanDown20th()
        {
            // data
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();

            for (var i = 0; i < 19; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(false, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }
    }
}