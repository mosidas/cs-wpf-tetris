namespace TetrisLogic
{
    public interface IBlocksPoolManager
    {
        public void Reset();
        public Block TakeNextBlock();
    }
}
