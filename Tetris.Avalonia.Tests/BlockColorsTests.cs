using Avalonia.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Avalonia.Rendering;
using Tetris.Domain;

namespace Tetris.Avalonia.Tests
{
    [TestClass]
    public class BlockColorsTests
    {
        [TestMethod]
        public void ColorFor_I_ReturnsLightBlue() =>
            Assert.AreEqual(Colors.LightBlue, BlockColors.ColorFor(BlockTypes.I));

        [TestMethod]
        public void ColorFor_O_ReturnsYellow() =>
            Assert.AreEqual(Colors.Yellow, BlockColors.ColorFor(BlockTypes.O));

        [TestMethod]
        public void ColorFor_S_ReturnsGreen() =>
            Assert.AreEqual(Colors.Green, BlockColors.ColorFor(BlockTypes.S));

        [TestMethod]
        public void ColorFor_Z_ReturnsRed() =>
            Assert.AreEqual(Colors.Red, BlockColors.ColorFor(BlockTypes.Z));

        [TestMethod]
        public void ColorFor_J_ReturnsBlue() =>
            Assert.AreEqual(Colors.Blue, BlockColors.ColorFor(BlockTypes.J));

        [TestMethod]
        public void ColorFor_L_ReturnsOrange() =>
            Assert.AreEqual(Colors.Orange, BlockColors.ColorFor(BlockTypes.L));

        [TestMethod]
        public void ColorFor_T_ReturnsPurple() =>
            Assert.AreEqual(Colors.Purple, BlockColors.ColorFor(BlockTypes.T));

        [TestMethod]
        public void ColorFor_Nothing_ReturnsEmptyDefault() =>
            Assert.AreEqual(BlockColors.Empty, BlockColors.ColorFor(BlockTypes.nothing));

        [TestMethod]
        public void Empty_IsDarkGray() =>
            Assert.AreEqual(Colors.DarkGray, BlockColors.Empty);

        [TestMethod]
        public void ColorFor_IsTotal_EveryBlockTypeMapped()
        {
            // 全 BlockTypes に対して例外なく色を返す(全域関数)。空種別のみ既定色に落ちる。
            foreach (BlockTypes type in System.Enum.GetValues<BlockTypes>())
            {
                var color = BlockColors.ColorFor(type);
                if (type == BlockTypes.nothing)
                {
                    Assert.AreEqual(BlockColors.Empty, color, $"{type} は既定色であるべき");
                }
                else
                {
                    Assert.AreNotEqual(BlockColors.Empty, color, $"{type} は固有色を持つべき");
                }
            }
        }
    }
}
