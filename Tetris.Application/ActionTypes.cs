namespace Tetris.Application
{
    /// <summary>
    /// 旧 GameManager 互換の操作種別。互換ファサードの入力型として維持する。
    /// </summary>
    public enum ActionTypes
    {
        nothing = 0,
        moveLeft,
        moveRight,
        moveDown,
        rotateLeft,
        rotateRight,
        hold,
        hardDrop,
    }
}
