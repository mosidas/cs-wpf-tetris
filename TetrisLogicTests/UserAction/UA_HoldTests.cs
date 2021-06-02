using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UA_HoldTests
    {
        [TestMethod()]
        public void ActionTest_InitHold()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            var act = new UA_Hold();
            act.Action(ref field, ref block, ref holdBlock);

            Assert.AreEqual(BlockTypes.nothing, block.BlockType);
            Assert.AreEqual(BlockTypes.O, holdBlock.BlockType);
            Assert.AreEqual(false, holdBlock.CanSwap);
        }

        [TestMethod()]
        public void ActionTest_Swap()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.I);
            var field = new Field();

            var act = new UA_Hold();
            act.Action(ref field, ref block, ref holdBlock);

            Assert.AreEqual(BlockTypes.I, block.BlockType);
            Assert.AreEqual(BlockTypes.O, holdBlock.BlockType);
            Assert.AreEqual(false, holdBlock.CanSwap);
        }

        [TestMethod()]
        public void CanActionTest1()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            var act = new UA_Hold();
            var ret = act.CanAction(field, block, holdBlock);

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest2()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.I,false);
            var field = new Field();

            var act = new UA_Hold();
            var ret = act.CanAction(field, block, holdBlock);

            Assert.AreEqual(false, ret);
        }

        [TestMethod()]
        public void CanActionTest3()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.I);
            var field = new Field();

            var act = new UA_Hold();
            act.Action(ref field, ref block, ref holdBlock);
            var ret = act.CanAction(field, block, holdBlock);

            Assert.AreEqual(false, ret);
        }
    }
}