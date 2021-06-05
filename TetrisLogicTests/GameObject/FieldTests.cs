using TetrisLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisLogic.Tests
{
    [TestClass()]
    public class FieldTests
    {
        [TestMethod()]
        public void InitFieldTest()
        {
            Field f = new Field();
            f.InitField();
            var expect =
 @"empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty
empty,empty,empty,empty,empty,empty,empty,empty,empty,empty";

            Assert.AreEqual(expect, f.DrawField());
        }

        [TestMethod()]
        public void GetFieldBlockPointAndTypePairsTest_InitField()
        {
            Field f = new Field();
            f.InitField();
            var ret = f.GetFieldBlockPointAndTypePairs();

            Assert.AreEqual(0, ret.Count);
        }

        [TestMethod()]
        public void GetFieldBlockPointAndTypePairsTest_1()
        {
            var init = new FieldTypes[,]
            {
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.fixedBlock,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
                { FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty,FieldTypes.empty },
            };

            Field f = new Field();
            f.InitField(init);
            var ret = f.GetFieldBlockPointAndTypePairs();
            Assert.AreEqual(3, ret[0].Item1.X);
            Assert.AreEqual(2, ret[0].Item1.Y);
            Assert.AreEqual(BlockTypes.I, ret[0].Item2);


        }
    }
}