using System;
using System.Diagnostics;
using Avalonia.Threading;

namespace Tetris.Avalonia.Game
{
    /// <summary>
    /// ゲームループ。約 16ms 間隔の <see cref="DispatcherTimer"/> で駆動し、各 tick で
    /// <see cref="Stopwatch"/> による実経過時間を測って進行へ渡す。タイマ間隔がゆらいでも
    /// 実経過時間を渡すため落下速度は実時間基準で一定に保たれる(固定フレーム量で進めない)。
    /// 開始/再開時は Stopwatch を再基準化し、停止中の経過時間を持ち越さない。
    /// </summary>
    public sealed class GameLoop
    {
        private readonly DispatcherTimer _timer;
        private readonly Stopwatch _stopwatch = new();
        private readonly Action<TimeSpan> _onTick;

        public GameLoop(Action<TimeSpan> onTick)
        {
            _onTick = onTick;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            _timer.Tick += OnTimerTick;
        }

        public bool IsRunning => _timer.IsEnabled;

        private void OnTimerTick(object? sender, EventArgs e)
        {
            var delta = _stopwatch.Elapsed;
            _stopwatch.Restart();
            _onTick(delta);
        }

        /// <summary>ループを開始/再開する。Stopwatch を再基準化して停止中の経過を持ち越さない。</summary>
        public void Start()
        {
            _stopwatch.Restart();
            _timer.Start();
        }

        /// <summary>ループを停止する。</summary>
        public void Stop()
        {
            _timer.Stop();
            _stopwatch.Reset();
        }
    }
}
