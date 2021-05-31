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
    public class UA_MoveDownTests
    {
        [TestMethod()]
        public void ActionTest_Normal()
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
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            var ret = act.CanAction(field, block);

            Assert.AreEqual(ret, true);
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
            for (var i = 0; i < 18; i++)
            {
                var ret1 = act.CanAction(field, block);
                act.Action(ref field, ref block, ref holdBlock);
                Assert.AreEqual(ret1, true);
            }
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanNotDownAfter18Down()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();

            for(var i = 0;i<18;i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            var ret = act.CanAction(field, block);
            Assert.AreEqual(ret, false);
        }

        [TestMethod()]
        public void CanActionTest_TBlockCanDown17th()
        {
            // data
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            for (var i = 0; i < 17; i++)
            {
                var ret1 = act.CanAction(field, block);
                act.Action(ref field, ref block, ref holdBlock);
                Assert.AreEqual(ret1, true);
            }
        }

        [TestMethod()]
        public void CanActionTest_TBlockCanNotDownAfter17Down()
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

            var ret = act.CanAction(field, block);
            Assert.AreEqual(ret, false);
        }

        [TestMethod()]
        public void CanActionTest_IBlockCanDown16th()
        {
            // data
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();
            for (var i = 0; i < 16; i++)
            {
                var ret1 = act.CanAction(field, block);
                act.Action(ref field, ref block, ref holdBlock);
                Assert.AreEqual(ret1, true);
            }
        }

        [TestMethod()]
        public void CanActionTest_IBlockCanNotDownAfter16Down()
        {
            // data
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_MoveDown();

            for (var i = 0; i < 16; i++)
            {
                act.Action(ref field, ref block, ref holdBlock);
            }

            var ret = act.CanAction(field, block);
            Assert.AreEqual(ret, false);
        }
    }
}