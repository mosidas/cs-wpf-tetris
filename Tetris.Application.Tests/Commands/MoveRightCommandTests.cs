using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests.Commands
{
    [TestClass()]
    public class MoveRightCommandTests
    {
        private static GameSession Session(Field field, Block current, Block hold)
        {
            var s = new GameSession(field, new BlocksPoolManagerDummy());
            s.CurrentBlock = current;
            s.HoldBlock = hold;
            return s;
        }

        [TestMethod()]
        public void ActionTest_LocationXIsplus1()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);
            var act = new MoveRightCommand();

            var before = session.CurrentBlock.Location;
            act.Execute(session);
            var after = session.CurrentBlock.Location;

            Assert.AreEqual(before.X + 1, after.X);
            Assert.AreEqual(before.Y, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_CanMoveRightInInitState()
        {
            var block = new Block(BlockTypes.O);
            var field = new Field();
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new MoveRightCommand();
            var ret = act.CanExecute(session);

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanMoveRight5th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveRightCommand();
            for (var i = 0; i < 4; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanNotMoveRight6th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveRightCommand();
            for (var i = 0; i < 5; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(false, act.CanExecute(session));
        }
    }
}
