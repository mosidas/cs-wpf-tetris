using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic
{
    public class BlocksPoolManager : IBlocksPoolManager
    {
        private List<Block> BlocksPool { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public BlocksPoolManager()
        {
            BlocksPool = new List<Block>();
            Reset();
        }

        /// <summary>
        /// プールの初期化
        /// </summary>
        public void Reset()
        {
            BlocksPool = new List<Block>();
        }

        /// <summary>
        /// プールから次のブロックを取り出す。さらにプールを補充する。
        /// </summary>
        /// <returns></returns>
        public Block TakeNextBlock()
        {
            CreateBlocksPool();
            var nextBlock = BlocksPool[0];
            BlocksPool.RemoveAt(0);
            return nextBlock;
        }

        public List<Block> GetNextBlocksPool(int count)
        {
            var pool = BlocksPool.Take(Math.Min(count, BlocksPool.Count)).ToList();
            return pool;
        }

        private void CreateBlocksPool()
        {
            if (BlocksPool.Count > 6)
            {
                return;
            }

            var list = new List<Block>();
            list.Add(new Block(SystemProperty.BlockType.O));
            list.Add(new Block(SystemProperty.BlockType.I));
            list.Add(new Block(SystemProperty.BlockType.T));
            list.Add(new Block(SystemProperty.BlockType.J));
            list.Add(new Block(SystemProperty.BlockType.L));
            list.Add(new Block(SystemProperty.BlockType.Z));
            list.Add(new Block(SystemProperty.BlockType.S));

            BlocksPool.AddRange(list.OrderBy(a => Guid.NewGuid()).ToList());
        }
    }
}
