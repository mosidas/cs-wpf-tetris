using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests.Commands
{
    [TestClass()]
    public class HoldCommandTests
    {
        private static GameSession Session(Field field, Block current, Block hold)
        {
            var s = new GameSession(field, new BlocksPoolManagerDummy());
            s.CurrentBlock = current;
            s.HoldBlock = hold;
            return s;
        }

        [TestMethod()]
        public void ActionTest_InitHold()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HoldCommand();
            act.Execute(session);

            Assert.AreEqual(BlockTypes.nothing, session.CurrentBlock.BlockType);
            Assert.AreEqual(BlockTypes.O, session.HoldBlock.BlockType);
            Assert.AreEqual(false, session.HoldBlock.CanSwap);
        }

        [TestMethod()]
        public void ActionTest_Swap()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.I);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HoldCommand();
            act.Execute(session);

            Assert.AreEqual(BlockTypes.I, session.CurrentBlock.BlockType);
            Assert.AreEqual(BlockTypes.O, session.HoldBlock.BlockType);
            Assert.AreEqual(false, session.HoldBlock.CanSwap);
        }

        [TestMethod()]
        public void CanActionTest1()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.nothing);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HoldCommand();
            var ret = act.CanExecute(session);

            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void CanActionTest2()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.I,false);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HoldCommand();
            var ret = act.CanExecute(session);

            Assert.AreEqual(false, ret);
        }

        [TestMethod()]
        public void CanActionTest3()
        {
            var block = new Block(BlockTypes.O);
            var holdBlock = new Block(BlockTypes.I);
            var field = new Field();
            var session = Session(field, block, holdBlock);

            var act = new HoldCommand();
            act.Execute(session);
            var ret = act.CanExecute(session);

            Assert.AreEqual(false, ret);
        }
    }
}
