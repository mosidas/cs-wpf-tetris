using Avalonia.Media;
using Tetris.Domain;

namespace Tetris.Avalonia.Rendering
{
    /// <summary>
    /// ブロック種別 → 表示色 の全域マッピング。WPF 版の対応(I=LightBlue, O=Yellow,
    /// S=Green, Z=Red, J=Blue, L=Orange, T=Purple, 空=DarkGray)を踏襲する。
    /// 盤面・ホールド・ネクストで共有する単一の集約点。
    /// </summary>
    public static class BlockColors
    {
        /// <summary>空セル/未定義ブロックの既定色。</summary>
        public static Color Empty { get; } = Colors.DarkGray;

        public static Color ColorFor(BlockTypes type) => type switch
        {
            BlockTypes.I => Colors.LightBlue,
            BlockTypes.O => Colors.Yellow,
            BlockTypes.S => Colors.Green,
            BlockTypes.Z => Colors.Red,
            BlockTypes.J => Colors.Blue,
            BlockTypes.L => Colors.Orange,
            BlockTypes.T => Colors.Purple,
            _ => Empty,
        };
    }
}
