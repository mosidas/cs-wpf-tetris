using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests.Commands
{
    [TestClass()]
    public class HardDropCommandTests
    {
        private static GameSession Session(Field field, Block current, Block hold)
        {
            var s = new GameSession(field, new BlocksPoolManagerDummy());
            s.CurrentBlock = current;
            s.HoldBlock = hold;
            return s;
        }

        [TestMethod()]
        public void ActionTest_Normal()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HardDropCommand();

            var before = session.CurrentBlock.Location;

            act.Execute(session);

            var after = session.CurrentBlock.Location;

            Assert.AreEqual(before.X, after.X);
            Assert.AreEqual(before.Y + 18, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_BlockCanDropInInitState()
        {
            var block = new Block(BlockTypes.O);
            var field = new Field();
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new HardDropCommand();
            var ret = act.CanExecute(session);

            Assert.AreEqual(ret, true);
        }

        [TestMethod()]
        public void CanActionTest_BlockCanNotDropAfterDrop()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HardDropCommand();

            act.Execute(session);

            var ret = act.CanExecute(session);
            Assert.AreEqual(ret, false);
        }
    }
}
