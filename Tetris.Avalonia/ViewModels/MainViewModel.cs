using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Threading;
using Tetris.Application;
using Tetris.Avalonia.Game;
using Tetris.Domain;

namespace Tetris.Avalonia.ViewModels
{
    /// <summary>
    /// 表示用 ViewModel。ゲームルールは持たず、<see cref="GameSession"/> の状態を
    /// <see cref="GameStateSnapshot"/> 経由で表示形へ変換し、入力(コマンド適用・ポーズ等)を仲介する。
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private const int StartLevel = 1;
        private const double EffectDurationMs = 1000;

        private readonly GameSession _session;
        private readonly GameLoop _loop;
        private readonly DispatcherTimer _effectTimer;

        [ObservableProperty]
        private GameStateSnapshot? _snapshot;

        [ObservableProperty]
        private string _scoreText = "0";

        [ObservableProperty]
        private string _effectText = "";

        [ObservableProperty]
        private BlockTypes _holdBlockType = BlockTypes.nothing;

        [ObservableProperty]
        private BlockTypes _nextBlockType = BlockTypes.nothing;

        [ObservableProperty]
        private string _statusMessage = "";

        [ObservableProperty]
        private bool _isMessageVisible;

        public MainViewModel(GameSession session)
        {
            _session = session;
            _loop = new GameLoop(OnTick);
            _effectTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(EffectDurationMs) };
            _effectTimer.Tick += (_, _) => { _effectTimer.Stop(); EffectText = ""; };
            ShowStartMessage();
        }

        public bool IsGameOver => _session.IsGameOver;
        public bool IsPaused { get; private set; }

        /// <summary>ゲームを開始する(ゲームオーバー画面/初期状態からの開始)。</summary>
        public void Start(int level = StartLevel)
        {
            IsMessageVisible = false;
            IsPaused = false;
            _session.Start(level);
            ScoreText = "0";
            EffectText = "";
            Refresh();
            _loop.Start();
            OnPropertyChanged(nameof(IsGameOver));
        }

        /// <summary>一時停止する。</summary>
        public void Pause()
        {
            if (IsPaused || _session.IsGameOver)
            {
                return;
            }

            _loop.Stop();
            IsPaused = true;
            ShowPauseMessage();
        }

        /// <summary>一時停止から再開する。</summary>
        public void Resume()
        {
            if (!IsPaused)
            {
                return;
            }

            IsMessageVisible = false;
            IsPaused = false;
            _loop.Start();
        }

        /// <summary>リセット(再スタート)する。</summary>
        public void Reset() => Start(StartLevel);

        /// <summary>ユーザー入力コマンドを適用する。</summary>
        public void Apply(GameCommand command)
        {
            if (_session.IsGameOver || IsPaused)
            {
                return;
            }

            _session.Apply(command);
            Refresh();

            if (_session.IsGameOver)
            {
                _loop.Stop();
                ShowGameOverMessage();
                OnPropertyChanged(nameof(IsGameOver));
            }
        }

        private void OnTick(TimeSpan delta)
        {
            _session.Advance(delta);
            Refresh();

            if (_session.IsGameOver)
            {
                _loop.Stop();
                ShowGameOverMessage();
                OnPropertyChanged(nameof(IsGameOver));
            }
        }

        private void Refresh()
        {
            var snap = _session.Snapshot();
            Snapshot = snap;

            var scoreString = snap.Score.ToString();
            if (scoreString != ScoreText)
            {
                ScoreText = scoreString;
                ShowEffect(snap.TSpinType, snap.Line);
            }

            HoldBlockType = snap.HoldBlockType;
            NextBlockType = snap.NextBlockTypes.Count > 0 ? snap.NextBlockTypes[0] : BlockTypes.nothing;
        }

        private void ShowEffect(TSpinTypes tSpinType, int line)
        {
            var text = tSpinType switch
            {
                TSpinTypes.tMini => "T-Spin Mini!",
                TSpinTypes.tSpin when line == 1 => "T-Spin Single!",
                TSpinTypes.tSpin when line == 2 => "T-Spin Double!!",
                TSpinTypes.tSpin when line == 3 => "T-Spin Triple!!!",
                _ when line == 4 => "Tetris!!!",
                _ => "",
            };

            EffectText = text;
            _effectTimer.Stop();
            if (text.Length > 0)
            {
                _effectTimer.Start();
            }
        }

        private void ShowStartMessage()
        {
            StatusMessage = "ゲーム開始：Space　一時停止：P　終了：Esc";
            IsMessageVisible = true;
        }

        private void ShowPauseMessage()
        {
            StatusMessage = "ゲーム再開：Space　リセット：R　終了：Esc";
            IsMessageVisible = true;
        }

        private void ShowGameOverMessage()
        {
            StatusMessage = "ゲームオーバー　開始：Space　終了：Esc";
            IsMessageVisible = true;
        }
    }
}
