using Tetris.Domain;

namespace Tetris.Application
{
    public sealed class HoldCommand : IGameCommand
    {
        public bool CanExecute(GameSession session)
        {
            if (session.HoldBlock.BlockType == BlockTypes.nothing)
            {
                return true;
            }
            else
            {
                return session.HoldBlock.CanSwap;
            }
        }

        public void Execute(GameSession session)
        {
            if (session.HoldBlock.BlockType == BlockTypes.nothing)
            {
                session.HoldBlock = new Block(session.CurrentBlock.BlockType, false);
                session.CurrentBlock = new Block(BlockTypes.nothing);
            }
            else
            {
                var tmp = new Block(session.HoldBlock.BlockType);
                session.HoldBlock = new Block(session.CurrentBlock.BlockType, false);
                session.CurrentBlock = new Block(tmp.BlockType);
            }
            session.CurrentBlock.TSpinType = TSpinTypes.notTSpin;
        }
    }
}
