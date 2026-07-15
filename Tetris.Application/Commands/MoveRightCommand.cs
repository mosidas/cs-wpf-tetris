using Tetris.Domain;

namespace Tetris.Application
{
    public sealed class MoveRightCommand : IGameCommand
    {
        public bool CanExecute(GameSession session)
        {
            foreach (var p in session.CurrentBlock.GetBlockRightPoints())
            {
                if (session.Field.GetFieldType(p.X + 1, p.Y) != FieldTypes.empty)
                {
                    return false;
                }
            }

            return true;
        }

        public void Execute(GameSession session)
        {
            session.CurrentBlock.MoveLocation(1, 0);
            session.CurrentBlock.TSpinType = TSpinTypes.notTSpin;
        }
    }
}
