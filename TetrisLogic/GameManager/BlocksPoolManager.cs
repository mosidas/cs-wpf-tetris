using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic
{
    public class BlocksPoolManager : IBlocksPoolManager
    {
        private List<Block> _blocksPool = new List<Block>();

        /// <summary>
        /// 初期化
        /// </summary>
        public BlocksPoolManager()
        {
            Reset();
        }

        /// <summary>
        /// プールの初期化
        /// </summary>
        public void Reset()
        {
            CreateBlocksPool();
        }

        /// <summary>
        /// プールから次のブロックを取り出す。さらにプールを補充する。
        /// </summary>
        /// <returns></returns>
        public Block TakeNextBlock()
        {
            CreateBlocksPool();
            var nextBlock = _blocksPool[0];
            _blocksPool.RemoveAt(0);
            return nextBlock;
        }

        public List<Block> GetNextBlocksPool(int count)
        {
            var pool = _blocksPool.Take(Math.Min(count, _blocksPool.Count)).ToList();
            return pool;
        }

        private void CreateBlocksPool()
        {
            if (_blocksPool.Count > 6)
            {
                return;
            }

            var list = new List<Block>();
            list.Add(new Block(BlockTypes.O));
            list.Add(new Block(BlockTypes.I));
            list.Add(new Block(BlockTypes.T));
            list.Add(new Block(BlockTypes.J));
            list.Add(new Block(BlockTypes.L));
            list.Add(new Block(BlockTypes.Z));
            list.Add(new Block(BlockTypes.S));

            _blocksPool.AddRange(list.OrderBy(a => Guid.NewGuid()).ToList());
        }
    }
}
