using Tetris.Domain;

namespace Tetris.Application
{
    /// <summary>
    /// タイミング非依存のゲーム進行ルール本体。可変状態(Field/CurrentBlock/HoldBlock)を保持する。
    /// 進行(Start/Apply/Advance)は後続で追加する。
    /// </summary>
    public sealed class GameSession
    {
        private readonly IBlocksPoolManager _blocksPoolManager;

        public Field Field { get; }
        public Block CurrentBlock { get; set; }
        public Block HoldBlock { get; set; }

        public GameSession(Field field, IBlocksPoolManager pool)
        {
            Field = field;
            _blocksPoolManager = pool;
            CurrentBlock = new Block(BlockTypes.nothing);
            HoldBlock = new Block(BlockTypes.nothing);
        }
    }
}
