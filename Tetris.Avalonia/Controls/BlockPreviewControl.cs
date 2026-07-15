using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Tetris.Domain;
using Tetris.Avalonia.Rendering;

namespace Tetris.Avalonia.Controls
{
    /// <summary>
    /// ホールド/ネクスト用の小パネル。ブロック種別を 4 セルの正準形で描画する。
    /// セル配置は WPF 版のホールド/ネクスト レイアウトを踏襲する。
    /// </summary>
    public sealed class BlockPreviewControl : Control
    {
        private const int GridSize = 4;
        private const double CellGap = 1.0;

        public static readonly StyledProperty<BlockTypes> BlockTypeProperty =
            AvaloniaProperty.Register<BlockPreviewControl, BlockTypes>(nameof(BlockType));

        static BlockPreviewControl()
        {
            AffectsRender<BlockPreviewControl>(BlockTypeProperty);
        }

        public BlockTypes BlockType
        {
            get => GetValue(BlockTypeProperty);
            set => SetValue(BlockTypeProperty, value);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            context.FillRectangle(Brushes.Black, new Rect(Bounds.Size));

            var type = BlockType;
            if (type == BlockTypes.nothing)
            {
                return; // 未使用(ホールド空)は何も描かない。
            }

            var cell = Math.Min(Bounds.Width, Bounds.Height) / GridSize;
            if (cell <= 0)
            {
                return;
            }

            var originX = (Bounds.Width - cell * GridSize) / 2;
            var originY = (Bounds.Height - cell * GridSize) / 2;
            var brush = new SolidColorBrush(BlockColors.ColorFor(type));

            foreach (var (col, row) in OffsetsFor(type))
            {
                var rect = new Rect(
                    originX + col * cell + CellGap,
                    originY + row * cell + CellGap,
                    Math.Max(0, cell - CellGap * 2),
                    Math.Max(0, cell - CellGap * 2));
                context.FillRectangle(brush, rect);
            }
        }

        // WPF 版 SetNextBlock / SetHoldBlock の Grid 配置(列,行)を踏襲。
        private static (int Col, int Row)[] OffsetsFor(BlockTypes type) => type switch
        {
            BlockTypes.I => new[] { (1, 0), (1, 1), (1, 2), (1, 3) },
            BlockTypes.T => new[] { (0, 1), (1, 1), (2, 1), (1, 2) },
            BlockTypes.J => new[] { (1, 0), (1, 1), (1, 2), (0, 2) },
            BlockTypes.L => new[] { (1, 0), (1, 1), (1, 2), (2, 2) },
            BlockTypes.S => new[] { (1, 1), (2, 1), (0, 2), (1, 2) },
            BlockTypes.Z => new[] { (0, 1), (1, 1), (1, 2), (2, 2) },
            BlockTypes.O => new[] { (0, 1), (0, 2), (1, 1), (1, 2) },
            _ => Array.Empty<(int, int)>(),
        };
    }
}
