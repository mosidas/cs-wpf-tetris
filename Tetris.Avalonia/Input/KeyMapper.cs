using Avalonia.Input;
using Tetris.Application;

namespace Tetris.Avalonia.Input
{
    /// <summary>
    /// Avalonia のキー入力をゲームコマンドへ変換する純粋関数群。WPF のキー割当を踏襲する
    /// (上=ハードドロップ, 下/左/右=移動, Z=左回転, X=右回転, Space=ホールド)。
    /// 一時停止/再開/リセット/終了などの制御キーはビュー側の状態機械が扱う(ここには含めない)。
    /// </summary>
    public static class KeyMapper
    {
        /// <summary>プレイ中のキー → ゲームコマンド。未対応キーは <see cref="GameCommand.nothing"/>。</summary>
        public static GameCommand ToGameCommand(Key key) => key switch
        {
            Key.Up => GameCommand.hardDrop,
            Key.Down => GameCommand.moveDown,
            Key.Left => GameCommand.moveLeft,
            Key.Right => GameCommand.moveRight,
            Key.Z => GameCommand.rotateLeft,
            Key.X => GameCommand.rotateRight,
            Key.Space => GameCommand.hold,
            _ => GameCommand.nothing,
        };

        /// <summary>
        /// 1 回の押下で 1 回だけ適用すべき離散操作か(ハードドロップ・回転・ホールド)。
        /// 移動系(下/左/右)は OS のキーリピートによる連続適用を許す。
        /// </summary>
        public static bool IsDiscrete(Key key) =>
            key is Key.Up or Key.Z or Key.X or Key.Space;
    }
}
