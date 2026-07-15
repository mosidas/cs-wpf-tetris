using Avalonia.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Avalonia.Input;

namespace Tetris.Avalonia.Tests
{
    [TestClass]
    public class KeyMapperTests
    {
        [DataTestMethod]
        [DataRow(Key.Up, GameCommand.hardDrop)]
        [DataRow(Key.Down, GameCommand.moveDown)]
        [DataRow(Key.Left, GameCommand.moveLeft)]
        [DataRow(Key.Right, GameCommand.moveRight)]
        [DataRow(Key.Z, GameCommand.rotateLeft)]
        [DataRow(Key.X, GameCommand.rotateRight)]
        [DataRow(Key.Space, GameCommand.hold)]
        public void ToGameCommand_MapsPlayKeys(Key key, GameCommand expected) =>
            Assert.AreEqual(expected, KeyMapper.ToGameCommand(key));

        [DataTestMethod]
        [DataRow(Key.A)]
        [DataRow(Key.P)]
        [DataRow(Key.R)]
        [DataRow(Key.Escape)]
        [DataRow(Key.Enter)]
        public void ToGameCommand_UnmappedKey_ReturnsNothing(Key key) =>
            Assert.AreEqual(GameCommand.nothing, KeyMapper.ToGameCommand(key));

        [DataTestMethod]
        [DataRow(Key.Up)]
        [DataRow(Key.Z)]
        [DataRow(Key.X)]
        [DataRow(Key.Space)]
        public void IsDiscrete_HardDropRotateHold_AreDiscrete(Key key) =>
            Assert.IsTrue(KeyMapper.IsDiscrete(key));

        [DataTestMethod]
        [DataRow(Key.Down)]
        [DataRow(Key.Left)]
        [DataRow(Key.Right)]
        public void IsDiscrete_MovementKeys_AreNotDiscrete(Key key) =>
            Assert.IsFalse(KeyMapper.IsDiscrete(key));
    }
}
