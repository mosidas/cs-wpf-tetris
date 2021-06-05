using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.Tests
{
    [TestClass()]
    public class ScoreManagerTests
    {
        [TestMethod()]
        public void ResetTest()
        {
            ScoreManager sc = new ScoreManager();
            sc.Reset();
            Assert.AreEqual(0, sc.Score);
            sc.Add(1, TSpinTypes.notTSpin, 1, false, false);
            sc.Reset();
            Assert.AreEqual(0, sc.Score);
        }

        [TestMethod()]
        public void AddTest_1Line_100()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1,TSpinTypes.notTSpin,1,false,false);
            Assert.AreEqual(100, sc.Score);
        }

        [TestMethod()]
        public void AddTest_2Line_200()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(2, TSpinTypes.notTSpin, 1, false, false);
            Assert.AreEqual(200, sc.Score);
        }

        [TestMethod()]
        public void AddTest_3Line_300()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(3, TSpinTypes.notTSpin, 1, false, false);
            Assert.AreEqual(300, sc.Score);
        }

        [TestMethod()]
        public void AddTest_4Line_500()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(4, TSpinTypes.notTSpin, 1, false, false);
            Assert.AreEqual(500, sc.Score);
        }

        [TestMethod()]
        public void AddTest_TS_300()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.tSpin, 1, false, false);
            Assert.AreEqual(300, sc.Score);
        }

        [TestMethod()]
        public void AddTest_TD_500()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(2, TSpinTypes.tSpin, 1, false, false);
            Assert.AreEqual(500, sc.Score);
        }

        [TestMethod()]
        public void AddTest_TT_700()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(3, TSpinTypes.tSpin, 1, false, false);
            Assert.AreEqual(700, sc.Score);
        }

        [TestMethod()]
        public void AddTest_Ren2_plus100()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 2, false, false);
            Assert.AreEqual(200, sc.Score);
        }

        [TestMethod()]
        public void AddTest_3Ren_plus100()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 3, false, false);
            Assert.AreEqual(200, sc.Score);
        }

        [TestMethod()]
        public void AddTest_4Ren_plus200()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 4, false, false);
            Assert.AreEqual(300, sc.Score);
        }

        [TestMethod()]
        public void AddTest_5Ren_plus200()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 5, false, false);
            Assert.AreEqual(300, sc.Score);
        }

        [TestMethod()]
        public void AddTest_6Ren_plus300()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 6, false, false);
            Assert.AreEqual(400, sc.Score);
        }

        [TestMethod()]
        public void AddTest_7Ren_plus300()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 7, false, false);
            Assert.AreEqual(400, sc.Score);
        }

        [TestMethod()]
        public void AddTest_8Ren_plus400()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 8, false, false);
            Assert.AreEqual(500, sc.Score);
        }

        [TestMethod()]
        public void AddTest_9Ren_plus400()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 9, false, false);
            Assert.AreEqual(500, sc.Score);
        }

        [TestMethod()]
        public void AddTest_10Ren_plus400()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 10, false, false);
            Assert.AreEqual(500, sc.Score);
        }

        [TestMethod()]
        public void AddTest_11RenOver_plus500()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 11, false, false);
            Assert.AreEqual(600, sc.Score);
            sc.Reset();
            sc.Add(1, TSpinTypes.notTSpin, 100, false, false);
            Assert.AreEqual(600, sc.Score);
        }

        [TestMethod()]
        public void AddTest_BTB_plus100()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.tMini, 1, true, false);
            Assert.AreEqual(200, sc.Score);
        }

        [TestMethod()]
        public void AddTest_AlllClear_plus1000()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.tMini, 1, false, true);
            Assert.AreEqual(1100, sc.Score);
        }

        [TestMethod()]
        public void AddTest_4Line_11Ren_AllClear_2000()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(4, TSpinTypes.notTSpin, 11, false, true);
            Assert.AreEqual(2000, sc.Score);
        }

        [TestMethod()]
        public void AddTest_TT_8Ren_BTB_1200()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(3, TSpinTypes.tSpin, 8, true, false);
            Assert.AreEqual(1200, sc.Score);
        }

        [TestMethod()]
        public void AddTest_1Line_0To3Ren_500()
        {
            ScoreManager sc = new ScoreManager();
            sc.Add(1, TSpinTypes.notTSpin, 1, false, false);
            sc.Add(1, TSpinTypes.notTSpin, 2, false, false);
            sc.Add(1, TSpinTypes.notTSpin, 3, false, false);
            Assert.AreEqual(500, sc.Score);
        }
    }
}