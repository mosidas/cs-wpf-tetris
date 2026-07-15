using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests.Commands
{
    [TestClass()]
    public class RotateLeftCommandTests
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

            var act = new RotateLeftCommand();

            act.Execute(session);
            var expect =
@"0,1,0
1,1,0
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

            var act = new RotateLeftCommand();

            act.Execute(session);
            var expect =
@"0,1,0,0
0,1,0,0
0,1,0,0
0,1,0,0";
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

            var act = new RotateLeftCommand();

            act.Execute(session);
            var expect =
@"0,1,0
1,1,0
1,0,0";
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

            var act = new RotateLeftCommand();

            act.Execute(session);
            var expect =
@"1,0,0
1,1,0
0,1,0";
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

            var act = new RotateLeftCommand();

            act.Execute(session);
            var expect =
@"0,1,0
0,1,0
1,1,0";
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

            var act = new RotateLeftCommand();

            act.Execute(session);
            var expect =
@"1,1,0
0,1,0
0,1,0";
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

            var act = new RotateLeftCommand();

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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
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

            var act = new RotateLeftCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_IBlock_ByTheWall()
        {
            var block = new Block(BlockTypes.I);
            var field = new Field();

            block.RotateRight();
            block.MoveLocation(4, 2);
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new RotateLeftCommand();
            Assert.AreEqual(true, act.CanExecute(session));
        }
    }
}
