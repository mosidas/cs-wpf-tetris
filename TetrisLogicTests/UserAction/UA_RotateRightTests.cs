using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UA_RotateRightTests
    {
        [TestMethod()]
        public void ActionTest_TBlock()
        {
            // data
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect = 
@"0,1,0
0,1,1
0,1,0";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_IBlock()
        {
            // data
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect =
@"0,0,1,0
0,0,1,0
0,0,1,0
0,0,1,0";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_ZBlock()
        {
            // data
            var block = new Block(BlockTypes.Z);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect =
@"0,0,1
0,1,1
0,1,0";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_SBlock()
        {
            // data
            var block = new Block(BlockTypes.S);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect =
@"0,1,0
0,1,1
0,0,1";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_JBlock()
        {
            // data
            var block = new Block(BlockTypes.J);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect =
@"0,1,1
0,1,0
0,1,0";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_LBlock()
        {
            // data
            var block = new Block(BlockTypes.L);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect =
@"0,1,0
0,1,0
0,1,1";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_OBlock()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();

            // do
            act.Action(ref field, ref block, ref holdBlock);
            var expect =
@"1,1
1,1";
            var actual = StringDraw(block._Block);
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void CanActionTest_OBlock()
        {
            // data
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_TBlock_Normal()
        {
            // data
            var block = new Block(BlockTypes.T);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(0, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_TBlock_ByTheLeftWall()
        {
            // data
            var block = new Block(BlockTypes.T);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(-4, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_TBlock_ByTheRightWall()
        {
            // data
            var block = new Block(BlockTypes.T);
            var field = new Field();

            block.RotateLeft();
            block.MoveLocation(5, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_LBlock_Normal()
        {
            // data
            var block = new Block(BlockTypes.L);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(0, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_LBlock_ByTheLeftWall()
        {
            // data
            var block = new Block(BlockTypes.L);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(-4, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_LBlock_ByTheRightWall()
        {
            // data
            var block = new Block(BlockTypes.L);
            var field = new Field();

            block.RotateLeft();
            block.MoveLocation(5, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_IBlock_Normal()
        {
            // data
            var block = new Block(BlockTypes.I);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(0, 2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        [TestMethod()]
        public void CanActionTest_IBlock_ByTheWall()
        {
            // data
            var block = new Block(BlockTypes.I);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(4,2);

            // target
            var act = new UA_RotateRight();
            Assert.AreEqual(true, act.CanAction(field, block));
        }

        private string StringDraw(int[,] array)
        {
            var ret = new List<string>();

            for (int i = 0; i < array.GetLength(0); i++)
            {
                var tmp = new List<string>();
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    tmp.Add(array[i, j].ToString());
                }

                ret.Add(string.Join(",",tmp));
            }

            var rret = string.Join("\r\n", ret);

            return rret;
        }
    }
}