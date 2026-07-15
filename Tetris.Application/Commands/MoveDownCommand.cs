using Tetris.Domain;

namespace Tetris.Application
{
    public sealed class MoveDownCommand : IGameCommand
    {
        public bool CanExecute(GameSession session)
        {
            foreach (var p in session.CurrentBlock.GetBlockBottomPoints())
            {
                if (session.Field.GetFieldType(p.X, p.Y + 1) != FieldTypes.empty)
                {
                    return false;
                }
            }

            return true;
        }

        public void Execute(GameSession session)
        {
            session.CurrentBlock.MoveLocation(0, 1);
        }
    }
}
