using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisLogic;
using static TetrisLogic.SystemProperty;

namespace TetrisLogicTests.Stub
{
    public class BlocksPoolManagerDummy : IBlocksPoolManager
    {
        public Block TakeNextBlock()
        {
            return new Block(BlockType.O);
        }

        public void Reset()
        {

        }

        public List<Block> GetNextBlocksPool(int count)
        {
            return null;
        }
    }
}
