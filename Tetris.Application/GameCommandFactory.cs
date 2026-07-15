namespace Tetris.Application
{
    /// <summary>
    /// GameCommand を IGameCommand に変換する Factory。Application 内部に隠蔽する(公開 API に露出しない)。
    /// </summary>
    internal sealed class GameCommandFactory
    {
        public IGameCommand Create(GameCommand command)
        {
            return command switch
            {
                GameCommand.moveLeft => new MoveLeftCommand(),
                GameCommand.moveRight => new MoveRightCommand(),
                GameCommand.moveDown => new MoveDownCommand(),
                GameCommand.rotateLeft => new RotateLeftCommand(),
                GameCommand.rotateRight => new RotateRightCommand(),
                GameCommand.hold => new HoldCommand(),
                GameCommand.hardDrop => new HardDropCommand(),
                _ => new NothingCommand(),
            };
        }
    }
}
