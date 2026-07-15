using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;

namespace Tetris.Application.Tests.Commands
{
    // 旧 UserActionFactoryTests を移行。GameCommandFactory は internal のため
    // InternalsVisibleTo(Tetris.Application.Tests)経由で列挙値→コマンド型の写像を検証する。
    [TestClass()]
    public class GameCommandFactoryTests
    {
        [TestMethod()]
        public void CreateUserActionTest_Nothig()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.nothing);
            Assert.AreEqual(typeof(NothingCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_MoveDown()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.moveDown);
            Assert.AreEqual(typeof(MoveDownCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_MoveLeft()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.moveLeft);
            Assert.AreEqual(typeof(MoveLeftCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_MoveRight()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.moveRight);
            Assert.AreEqual(typeof(MoveRightCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_RotateLeft()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.rotateLeft);
            Assert.AreEqual(typeof(RotateLeftCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_RotateRight()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.rotateRight);
            Assert.AreEqual(typeof(RotateRightCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_HardDrop()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.hardDrop);
            Assert.AreEqual(typeof(HardDropCommand).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_Hold()
        {
            var uaf = new GameCommandFactory();
            var ret = uaf.Create(GameCommand.hold);
            Assert.AreEqual(typeof(HoldCommand).Name, ret.GetType().Name);
        }
    }
}
