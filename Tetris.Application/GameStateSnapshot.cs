using System.Collections.Generic;
using Tetris.Domain;

namespace Tetris.Application
{
    /// <summary>
    /// 表示用の読み取り専用モデル(read model)。生成時点の GameSession 状態のスナップショット。
    /// 可変状態は露出しない。
    /// </summary>
    public readonly record struct GameStateSnapshot(
        int FieldWidth,
        int FieldHeight,
        bool IsGameOver,
        int Score,
        int Level,
        TSpinTypes TSpinType,
        int Line,
        BlockTypes CurrentBlockType,
        IReadOnlyList<Position> CurrentBlockPoints,
        IReadOnlyList<Position> GhostBlockPoints,
        IReadOnlyList<(Position, BlockTypes)> FieldPointAndTypePairs,
        IReadOnlyList<Position> FieldBlockPoints,
        IReadOnlyList<Position> FixedBlockPoints,
        BlockTypes HoldBlockType,
        IReadOnlyList<BlockTypes> NextBlockTypes);
}
