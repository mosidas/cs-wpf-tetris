using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UA_MoveRightTests
    {
        [TestMethod()]
        public void ActionTest_LocationXIsplus1()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var act = new UA_MoveRight();

            var before = block.Location;
            act.Action(ref field, ref block, ref holdBlock);
            var after = block.Location;

            Assert.AreEqual(before.X + 1, after.X);
            Assert.AreEqual(before.Y, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_CanMoveRightInInitState()
        {
            var block = new Block(BlockTypes.O);
            var field = new Field();

            var act = new UA_MoveRight();
            var ret = act.CanAction(field, block);

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanMoveRight5th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            var act = new UA_MoveRight();
            for (var i = 0; i < 4; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanNotMoveRight6th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            var act = new UA_MoveRight();
            for (var i = 0; i < 5; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(false, act.CanAction(field, block));
        }
    }
}