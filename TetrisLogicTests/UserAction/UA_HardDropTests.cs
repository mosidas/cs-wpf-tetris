using Microsoft.VisualStudio.TestTools.UnitTesting;
using TetrisLogic.UserAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UA_HardDropTests
    {
        [TestMethod()]
        public void ActionTest_Normal()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_HardDrop();

            // before
            var before = block.Location;

            // do
            act.Action(ref field, ref block, ref holdBlock);

            // after
            var after = block.Location;

            Assert.AreEqual(before.X, after.X);
            Assert.AreEqual(before.Y + 18, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_CanDropInInitState()
        {
            // data
            var block = new Block(BlockTypes.O);
            var field = new Field();

            // target
            var act = new UA_HardDrop();
            var ret = act.CanAction(field, block);

            Assert.AreEqual(ret, true);
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanNotDropAfterDrop()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_HardDrop();

            act.Action(ref field, ref block, ref holdBlock);

            var ret = act.CanAction(field, block);
            Assert.AreEqual(ret, false);
        }
    }
}