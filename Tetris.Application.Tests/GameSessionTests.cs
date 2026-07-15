using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Application;
using Tetris.Application.Tests.Stub;
using Tetris.Domain;

namespace Tetris.Application.Tests
{
    [TestClass()]
    public class GameSessionTests
    {
        private static GameSession NewSession()
        {
            return new GameSession(new Field(), new BlocksPoolManagerDummy());
        }

        // レベル L の落下間隔(ms)。GameSession の等価換算と同一式。
        private static double IntervalMs(int level)
        {
            return Math.Max(1, GameSession.FPS - level * 5) * (1000.0 / GameSession.FPS);
        }

        [TestMethod()]
        public void StartTest_InitializesState()
        {
            var session = NewSession();
            Assert.AreEqual(true, session.IsGameOver);

            session.Start(5);
            Assert.AreEqual(false, session.IsGameOver);
            Assert.AreEqual(5, session.Level);
            Assert.AreEqual(0, session.Field.GetFixedBlockPoints().Count);
            Assert.AreEqual(4, session.CurrentBlock.GetBlockPoints().Count);
            Assert.AreEqual(0, session.Score);
        }

        [TestMethod()]
        public void StartTest_ClampsLevelToUpperBound()
        {
            var session = NewSession();
            session.Start(150);
            Assert.AreEqual(99, session.Level);
        }

        [TestMethod()]
        public void StartTest_ClampsLevelToLowerBound()
        {
            var session = NewSession();
            session.Start(-3);
            Assert.AreEqual(0, session.Level);
        }

        [TestMethod()]
        public void ApplyTest_MoveDownMovesOneCell()
        {
            var session = NewSession();
            session.Start(0);
            var before = session.CurrentBlock.Location;

            var ret = session.Apply(GameCommand.moveDown);

            var after = session.CurrentBlock.Location;
            Assert.AreEqual(true, ret);
            Assert.AreEqual(before.X, after.X);
            Assert.AreEqual(before.Y + 1, after.Y);
        }

        [TestMethod()]
        public void ApplyTest_HoldRetiresCurrentBlock()
        {
            var session = NewSession();
            session.Start(0);

            session.Apply(GameCommand.hold);

            Assert.AreEqual(BlockTypes.O, session.HoldBlock.BlockType);
            Assert.AreEqual(false, session.HoldBlock.CanSwap);
            Assert.AreEqual(BlockTypes.O, session.CurrentBlock.BlockType);
        }

        [TestMethod()]
        public void ApplyTest_HardDropFixesBlockAndSpawnsNext()
        {
            var session = NewSession();
            session.Start(0);

            session.Apply(GameCommand.hardDrop);

            // O ブロック 1 個(4 セル)が固定され、新しいブロックがスポーンする。
            Assert.AreEqual(4, session.Field.GetFixedBlockPoints().Count);
            Assert.AreEqual(0, session.CurrentBlock.Location.Y);
            Assert.AreEqual(4, session.CurrentBlock.GetBlockPoints().Count);
        }

        [TestMethod()]
        public void ApplyTest_GameOverWhenSpawnBlocked()
        {
            var session = NewSession();
            session.Start(0);

            // 常に O を供給するダミーで積み上げると、やがてスポーン不可でゲームオーバーになる。
            for (var i = 0; i < 30 && !session.IsGameOver; i++)
            {
                session.Apply(GameCommand.hardDrop);
            }

            Assert.AreEqual(true, session.IsGameOver);
        }

        [TestMethod()]
        public void ApplyTest_NoStateChangeAfterGameOver()
        {
            var session = NewSession();
            session.Start(0);
            for (var i = 0; i < 30 && !session.IsGameOver; i++)
            {
                session.Apply(GameCommand.hardDrop);
            }
            Assert.AreEqual(true, session.IsGameOver);

            var fixedCountBefore = session.Field.GetFixedBlockPoints().Count;
            var currentBefore = session.CurrentBlock;

            var ret = session.Apply(GameCommand.moveDown);
            session.Advance(TimeSpan.FromSeconds(10));

            Assert.AreEqual(false, ret);
            Assert.AreEqual(true, session.IsGameOver);
            Assert.AreEqual(fixedCountBefore, session.Field.GetFixedBlockPoints().Count);
            Assert.AreSame(currentBefore, session.CurrentBlock);
        }

        [TestMethod()]
        public void AdvanceTest_Level0DoesNotDrop()
        {
            var session = NewSession();
            session.Start(0);
            var beforeY = session.CurrentBlock.Location.Y;

            session.Advance(TimeSpan.FromSeconds(10));

            Assert.AreEqual(beforeY, session.CurrentBlock.Location.Y);
        }

        [TestMethod()]
        public void AdvanceTest_NoGravityWithoutAdvance()
        {
            // 時間は Advance でのみ流れる。Apply(nothing) を繰り返しても自然落下しない(内部フレームカウンタを持たない)。
            var session = NewSession();
            session.Start(1);
            var beforeY = session.CurrentBlock.Location.Y;

            for (var i = 0; i < 100; i++)
            {
                session.Apply(GameCommand.nothing);
            }

            Assert.AreEqual(beforeY, session.CurrentBlock.Location.Y);
        }

        [TestMethod()]
        public void AdvanceTest_DropsOneCellWhenThresholdReached()
        {
            var session = NewSession();
            session.Start(1);
            var beforeY = session.CurrentBlock.Location.Y;

            session.Advance(TimeSpan.FromMilliseconds(IntervalMs(1) + 5));

            Assert.AreEqual(beforeY + 1, session.CurrentBlock.Location.Y);
        }

        [TestMethod()]
        public void AdvanceTest_AccumulatesSubThresholdDeltas()
        {
            var session = NewSession();
            session.Start(1);
            var beforeY = session.CurrentBlock.Location.Y;
            var half = IntervalMs(1) / 2;

            session.Advance(TimeSpan.FromMilliseconds(half));
            Assert.AreEqual(beforeY, session.CurrentBlock.Location.Y);

            session.Advance(TimeSpan.FromMilliseconds(half + 2));
            Assert.AreEqual(beforeY + 1, session.CurrentBlock.Location.Y);
        }

        [TestMethod()]
        public void AdvanceTest_ThresholdIsEquivalentToOldFrameConversion()
        {
            var session = NewSession();
            session.Start(1);
            var beforeY = session.CurrentBlock.Location.Y;

            // 落下間隔 = max(1, 60 - 1*5) フレーム × (1000/60) ms 直前では落下しない。
            session.Advance(TimeSpan.FromMilliseconds(IntervalMs(1) - 1));
            Assert.AreEqual(beforeY, session.CurrentBlock.Location.Y);

            // 閾値到達で 1 セル落下する。
            session.Advance(TimeSpan.FromMilliseconds(2));
            Assert.AreEqual(beforeY + 1, session.CurrentBlock.Location.Y);
        }
    }
}
