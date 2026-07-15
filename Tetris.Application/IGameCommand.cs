namespace Tetris.Application
{
    /// <summary>
    /// ユーザー操作 1 種を表す指令。ref 渡しを廃し、GameSession の可変状態を読み書きする。
    /// </summary>
    public interface IGameCommand
    {
        bool CanExecute(GameSession session);
        void Execute(GameSession session);
    }
}
