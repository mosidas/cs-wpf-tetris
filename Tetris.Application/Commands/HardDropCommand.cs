namespace Tetris.Application
{
    public sealed class HardDropCommand : IGameCommand
    {
        private readonly MoveDownCommand _moveDown;

        public HardDropCommand()
        {
            _moveDown = new MoveDownCommand();
        }

        public bool CanExecute(GameSession session)
        {
            return _moveDown.CanExecute(session);
        }

        public void Execute(GameSession session)
        {
            while (CanExecute(session))
            {
                _moveDown.Execute(session);
            }
        }
    }
}
