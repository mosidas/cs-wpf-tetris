using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UA_MoveLeftTests
    {
        [TestMethod()]
        public void ActionTest_LocationXIsMinus1()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var act = new UA_MoveLeft();

            var before = block.Location;
            act.Action(ref field, ref block, ref holdBlock);
            var after = block.Location;

            Assert.AreEqual(before.X - 1, after.X);
            Assert.AreEqual(before.Y, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_CanMoveleftInInitState()
        {
            var block = new Block(BlockTypes.O);
            var field = new Field();

            var act = new UA_MoveLeft();
            var ret = act.CanAction(field, block, new Block(BlockTypes.nothing));

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanMoveLeft3th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            var act = new UA_MoveLeft();
            for (var i = 0; i < 2; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void CanActionTest_OBlockCannotMoveLeft4th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            var act = new UA_MoveLeft();
            for (var i = 0; i < 3; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            Assert.AreEqual(false, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }
    }
}