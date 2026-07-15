namespace Tetris.Domain
{
    /// <summary>
    /// フィールド上の座標を表す値型。旧 Point 型と同一意味(整数の X/Y、負値・フィールド外可)。
    /// </summary>
    public readonly record struct Position(int X, int Y);
}
