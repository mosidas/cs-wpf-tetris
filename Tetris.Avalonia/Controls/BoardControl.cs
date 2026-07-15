using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Tetris.Application;
using Tetris.Domain;
using Tetris.Avalonia.Rendering;

namespace Tetris.Avalonia.Controls
{
    /// <summary>
    /// 盤面を <see cref="GameStateSnapshot"/> 駆動で直接描画するカスタムコントロール。
    /// 名前付きセル(FindName)を用いず、<see cref="Render"/> で 10×20 を毎回描く。
    /// </summary>
    public sealed class BoardControl : Control
    {
        private const double CellGap = 1.0;

        public static readonly StyledProperty<GameStateSnapshot?> SnapshotProperty =
            AvaloniaProperty.Register<BoardControl, GameStateSnapshot?>(nameof(Snapshot));

        static BoardControl()
        {
            AffectsRender<BoardControl>(SnapshotProperty);
        }

        public GameStateSnapshot? Snapshot
        {
            get => GetValue(SnapshotProperty);
            set => SetValue(SnapshotProperty, value);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            var snap = Snapshot;
            var cols = snap?.FieldWidth ?? 10;
            var rows = snap?.FieldHeight ?? 20;
            if (cols <= 0 || rows <= 0)
            {
                return;
            }

            var cell = Math.Min(Bounds.Width / cols, Bounds.Height / rows);
            if (cell <= 0)
            {
                return;
            }

            var originX = (Bounds.Width - cell * cols) / 2;
            var originY = (Bounds.Height - cell * rows) / 2;

            // 盤面背景(黒地 + 空セルを既定色で敷き詰め、セル間の隙間で格子を表現)。
            context.FillRectangle(Brushes.Black, new Rect(originX, originY, cell * cols, cell * rows));
            var emptyBrush = new SolidColorBrush(BlockColors.Empty);
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    DrawCell(context, emptyBrush, originX, originY, cell, x, y);
                }
            }

            if (snap is not { } s)
            {
                return; // null のときは空盤面のみ(例外を投げない)。
            }

            // ゴースト(現在ブロック色・不透明度 0.5)。
            var ghostBrush = new SolidColorBrush(BlockColors.ColorFor(s.CurrentBlockType), 0.5);
            foreach (var p in s.GhostBlockPoints)
            {
                if (InBounds(p, cols, rows))
                {
                    DrawCell(context, ghostBrush, originX, originY, cell, p.X, p.Y);
                }
            }

            // フィールドの確定/移動中ブロック(不透明度 1.0)。ゴーストに重なる場合は上書き。
            foreach (var (position, type) in s.FieldPointAndTypePairs)
            {
                if (InBounds(position, cols, rows))
                {
                    var brush = new SolidColorBrush(BlockColors.ColorFor(type));
                    DrawCell(context, brush, originX, originY, cell, position.X, position.Y);
                }
            }
        }

        private static bool InBounds(Position p, int cols, int rows) =>
            p.X >= 0 && p.X < cols && p.Y >= 0 && p.Y < rows;

        private static void DrawCell(DrawingContext context, IBrush brush, double originX, double originY, double cell, int x, int y)
        {
            var rect = new Rect(
                originX + x * cell + CellGap,
                originY + y * cell + CellGap,
                Math.Max(0, cell - CellGap * 2),
                Math.Max(0, cell - CellGap * 2));
            context.FillRectangle(brush, rect);
        }
    }
}
