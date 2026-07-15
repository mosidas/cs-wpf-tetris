namespace Tetris.Application
{
    public sealed class NothingCommand : IGameCommand
    {
        public bool CanExecute(GameSession session)
        {
            return true;
        }

        public void Execute(GameSession session)
        {
        }
    }
}
