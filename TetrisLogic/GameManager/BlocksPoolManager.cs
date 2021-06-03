using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisLogic
{
    /// <summary>
    /// 落下ブロックの順番を制御するクラス 落下ブロックの順番はワールドルール準拠
    /// </summary>
    public class BlocksPoolManager : IBlocksPoolManager
    {
        private List<Block> _blocksPool = new List<Block>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlocksPoolManager()
        {
            Reset();
        }

        /// <summary>
        /// 落下ブロックの順番を初期化する
        /// </summary>
        public void Reset()
        {
            _blocksPool = new List<Block>();
            CreateBlocksPool();
        }

        /// <summary>
        /// 次のブロックを取得する
        /// </summary>
        public Block TakeNextBlock()
        {
            CreateBlocksPool();
            var nextBlock = _blocksPool[0];
            _blocksPool.RemoveAt(0);
            return nextBlock;
        }

        /// <summary>
        /// プールにあるブロックを落ちてくる順に取得する
        /// </summary>
        public List<Block> GetNextBlocksPool()
        {
            var pool = _blocksPool.ToList();
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
