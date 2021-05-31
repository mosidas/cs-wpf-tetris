using System.Collections.Generic;

namespace TetrisLogic
{
    public interface IBlocksPoolManager
    {
        public void Reset();
        public Block TakeNextBlock();
        public List<Block> GetNextBlocksPool(int count);
    }
}
