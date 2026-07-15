using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests.Commands
{
    [TestClass()]
    public class RotateRightCommandTests
    {
        private static GameSession Session(Field field, Block current, Block hold)
        {
            var s = new GameSession(field, new BlocksPoolManagerDummy());
            s.CurrentBlock = current;
            s.HoldBlock = hold;
            return s;
        }

        [TestMethod()]
        public void ActionTest_TBlock()
        {
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"0,1,0
0,1,1
0,1,0";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_IBlock()
        {
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"0,0,1,0
0,0,1,0
0,0,1,0
0,0,1,0";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_ZBlock()
        {
            var block = new Block(BlockTypes.Z);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"0,0,1
0,1,1
0,1,0";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_SBlock()
        {
            var block = new Block(BlockTypes.S);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"0,1,0
0,1,1
0,0,1";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_JBlock()
        {
            var block = new Block(BlockTypes.J);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"0,1,1
0,1,0
0,1,0";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_LBlock()
        {
            var block = new Block(BlockTypes.L);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"0,1,0
0,1,0
0,1,1";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void ActionTest_OBlock()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();

            act.Execute(session);
            var expect =
@"1,1
1,1";
            var actual = session.CurrentBlock.DrawBlock();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void CanActionTest_OBlock()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_TBlock_Normal()
        {
            var block = new Block(BlockTypes.T);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(0, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_TBlock_ByTheLeftWall()
        {
            var block = new Block(BlockTypes.T);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(-4, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_TBlock_ByTheRightWall()
        {
            var block = new Block(BlockTypes.T);
            var field = new Field();

            block.RotateLeft();
            block.MoveLocation(5, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_LBlock_Normal()
        {
            var block = new Block(BlockTypes.L);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(0, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_LBlock_ByTheLeftWall()
        {
            var block = new Block(BlockTypes.L);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(-4, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_LBlock_ByTheRightWall()
        {
            var block = new Block(BlockTypes.L);
            var field = new Field();

            block.RotateLeft();
            block.MoveLocation(5, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_IBlock_Normal()
        {
            var block = new Block(BlockTypes.I);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(0, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_IBlock_ByTheWall()
        {
            var block = new Block(BlockTypes.I);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(4,2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateRightCommand();
            Assert.AreEqual(true, act.CanExecute(session));
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
            var session = Session(field, block, holdBlock);
            var act = new RotateRightCommand();

            // 壁蹴りで回転可能と判定される。
            Assert.AreEqual(true, act.CanExecute(session));

            act.Execute(session);

            // 判定した壁蹴り位置と実際の配置が一致し、固定ブロックにめり込まない。
            Assert.AreEqual(false, field.ExistsCollisionPoint(session.CurrentBlock));
            var actual = session.CurrentBlock.GetBlockPoints();
            var expect = new System.Collections.Generic.List<Position>
            {
                new Position(5, 11),
                new Position(6, 11),
                new Position(7, 11),
                new Position(8, 11),
            };
            CollectionAssert.AreEquivalent(expect, actual);
        }
    }
}
