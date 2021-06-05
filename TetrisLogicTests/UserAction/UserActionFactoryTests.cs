using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.UserAction.Tests
{
    [TestClass()]
    public class UserActionFactoryTests
    {
        [TestMethod()]
        public void CreateUserActionTest_Nothig()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.nothing);
            Assert.IsNull(ret);
        }

        [TestMethod()]
        public void CreateUserActionTest_MoveDown()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.moveDown);
            Assert.AreEqual(typeof(UA_MoveDown).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_MoveLeft()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.moveLeft);
            Assert.AreEqual(typeof(UA_MoveLeft).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_MoveRight()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.moveRight);
            Assert.AreEqual(typeof(UA_MoveRight).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_RotateLeft()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.rotateLeft);
            Assert.AreEqual(typeof(UA_RotateLeft).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_RotateRight()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.rotateRight);
            Assert.AreEqual(typeof(UA_RotateRight).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_HardDrop()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.hardDrop);
            Assert.AreEqual(typeof(UA_HardDrop).Name, ret.GetType().Name);
        }

        [TestMethod()]
        public void CreateUserActionTest_Hold()
        {
            UserActionFactory uaf = new UserActionFactory();
            var ret = uaf.CreateUserAction(ActionTypes.hold);
            Assert.AreEqual(typeof(UA_Hold).Name, ret.GetType().Name);
        }
    }
}