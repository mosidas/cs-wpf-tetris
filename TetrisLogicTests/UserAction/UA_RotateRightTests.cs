using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var actual = block.DrawBlock();
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
            var actual = block.DrawBlock();
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
            var actual = block.DrawBlock();
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
            var actual = block.DrawBlock();
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
            var actual = block.DrawBlock();
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
            var actual = block.DrawBlock();
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
            var actual = block.DrawBlock();
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
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
            Assert.AreEqual(true, act.CanAction(field, block, new Block(BlockTypes.nothing)));
        }

        [TestMethod()]
        public void ActionTest_IBlock_SRSKickFromWest_DoesNotCollide()
        {
            // data: west 向きの I ミノが west->north 回転で 5 番目の壁蹴り(state4)を要する盤面。
            // (5,12) と (5,13) の固定ブロックが基本回転と state1-3 を塞ぎ、state4 のみ成立する。
            var fieldState = new FieldTypes[20, 10];
            for (var row = 0; row < 20; row++)
            {
                for (var col = 0; col < 10; col++)
                {
                    fieldState[row, col] = FieldTypes.empty;
                }
            }
            fieldState[12, 5] = FieldTypes.fixedBlock;
            fieldState[13, 5] = FieldTypes.fixedBlock;

            var field = new Field();
            field.InitField(fieldState);

            var block = new Block(BlockTypes.I);
            block.RotateRight();
            block.RotateRight();
            block.RotateRight();
            block.MoveLocation(4, 12);

            var holdBlock = new Block(BlockTypes.nothing);
            var act = new UA_RotateRight();

            // 壁蹴りで回転可能と判定される。
            Assert.AreEqual(true, act.CanAction(field, block, holdBlock));

            act.Action(ref field, ref block, ref holdBlock);

            // 判定した壁蹴り位置と実際の配置が一致し、固定ブロックにめり込まない。
            Assert.AreEqual(false, field.ExistsCollisionPoint(block));
            var actual = block.GetBlockPoints();
            var expect = new System.Collections.Generic.List<System.Drawing.Point>
            {
                new System.Drawing.Point(5, 11),
                new System.Drawing.Point(6, 11),
                new System.Drawing.Point(7, 11),
                new System.Drawing.Point(8, 11),
            };
            CollectionAssert.AreEquivalent(expect, actual);
        }
    }
}