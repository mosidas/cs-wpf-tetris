using System.Collections.Generic;
using Tetris.Domain;

namespace Tetris.Application
{
    public interface IBlocksPoolManager
    {
        void Reset();
        Block TakeNextBlock();
        List<Block> GetNextBlocksPool();
    }
}
