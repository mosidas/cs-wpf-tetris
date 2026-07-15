using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests.Commands
{
    [TestClass()]
    public class MoveDownCommandTests
    {
        private static GameSession Session(Field field, Block current, Block hold)
        {
            var s = new GameSession(field, new BlocksPoolManagerDummy());
            s.CurrentBlock = current;
            s.HoldBlock = hold;
            return s;
        }

        [TestMethod()]
        public void ActionTest_LocationYIsPlus1()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();

            var before = session.CurrentBlock.Location;

            act.Execute(session);

            var after = session.CurrentBlock.Location;

            Assert.AreEqual(before.X, after.X);
            Assert.AreEqual(before.Y + 1, after.Y);
        }

        [TestMethod()]
        public void CanActionTest_InitStateCanDown()
        {
            var block = new Block(BlockTypes.O);
            var field = new Field();
            var session = Session(field, block, new Block(BlockTypes.nothing));

            var act = new MoveDownCommand();
            var ret = act.CanExecute(session);

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanDown18th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();
            for (var i = 0; i < 17; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_OBlockCanNotDown19th()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();

            for (var i = 0; i < 18; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(false, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_TBlockCanDown18th()
        {
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();
            for (var i = 0; i < 17; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_TBlockCanNotDown19th()
        {
            var block = new Block(BlockTypes.T);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();

            for (var i = 0; i < 18; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(false, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_IBlockCanDown19th()
        {
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();
            for (var i = 0; i < 18; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(true, act.CanExecute(session));
        }

        [TestMethod()]
        public void CanActionTest_IBlockCanNotCanDown20th()
        {
            var block = new Block(BlockTypes.I);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new MoveDownCommand();

            for (var i = 0; i < 19; i++)
            {
                act.Execute(session);
            }

            Assert.AreEqual(false, act.CanExecute(session));
        }
    }
}
