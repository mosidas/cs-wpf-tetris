using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Domain;

namespace Tetris.Application
{
    /// <summary>
    /// タイミング非依存のゲーム進行ルール本体。可変状態(Field/CurrentBlock/HoldBlock)と
    /// 進行状態(Score/Level/REN/B2B/重力アキュムレータ)を保持する。
    /// 内部に FPS・フレームカウンタを持たず、時間は Advance(TimeSpan) の引数としてのみ受け取る。
    /// </summary>
    public sealed class GameSession
    {
        /// <summary>
        /// フレームレート算出の基準値(旧 GameManager.FPS と同値)。
        /// </summary>
        public static readonly int FPS = 60;

        private readonly IBlocksPoolManager _blocksPoolManager;
        private readonly ScoreManager _scoreManager;
        private readonly GameCommandFactory _factory = new GameCommandFactory();

        public Field Field { get; }
        public Block CurrentBlock { get; set; }
        public Block HoldBlock { get; set; }

        public bool IsGameOver { get; private set; }
        public int Score { get { return _scoreManager.Score; } }
        public int Level { get; private set; }
        public TSpinTypes LastTSpin { get { return _beforeT; } }
        public int LastClearedLines { get { return _beforeLine; } }

        private double _gravityAccumulatorMs;
        private int _ren;
        private int _beforeLine;
        private TSpinTypes _beforeT;
        private int _beforeScore;
        private int _scoreDelta;

        public GameSession(Field field, IBlocksPoolManager pool)
        {
            Field = field;
            _blocksPoolManager = pool;
            _scoreManager = new ScoreManager();
            CurrentBlock = new Block(BlockTypes.nothing);
            HoldBlock = new Block(BlockTypes.nothing);
            IsGameOver = true;
        }

        /// <summary>
        /// レベルに応じた 1 セル自然落下の落下間隔(フレーム数)。旧 DownRate と等価。
        /// </summary>
        private int DownRateFrames
        {
            get { return Level == 0 ? 0 : Math.Max(1, FPS - Level * 5); }
        }

        /// <summary>
        /// 落下間隔(ms)。max(1, 60 - L*5) フレーム × (1000/60) ms。旧 DownRate × FrameRate と等価。
        /// </summary>
        private double GravityIntervalMs
        {
            get { return DownRateFrames * (1000.0 / FPS); }
        }

        /// <summary>
        /// ゲームを開始する。level は 0–99 にクランプする(レベル 0 は自然落下しない)。
        /// </summary>
        public void Start(int level = 1)
        {
            Level = Math.Min(Math.Max(level, 0), 99);
            IsGameOver = false;
            Field.InitField();
            _blocksPoolManager.Reset();
            CurrentBlock = _blocksPoolManager.TakeNextBlock();
            HoldBlock = new Block(BlockTypes.nothing);
            Field.UpdateField(CurrentBlock, false);
            _scoreManager.Reset();
            _gravityAccumulatorMs = 0;
            _ren = 0;
            _beforeLine = 0;
            _beforeT = TSpinTypes.notTSpin;
            _beforeScore = 0;
            _scoreDelta = 0;
        }

        /// <summary>
        /// コマンドを適用する(旧 Act + UpdateGameState 相当)。戻り値は下位アクションの成立可否(旧 canAction 相当)。
        /// </summary>
        public bool Apply(GameCommand command)
        {
            if (IsGameOver)
            {
                return false;
            }

            var cmd = _factory.Create(command);
            var canAction = cmd.CanExecute(this);
            if (canAction)
            {
                cmd.Execute(this);
            }

            UpdateGameState(command, canAction);
            return canAction;
        }

        /// <summary>
        /// 経過時間ぶん重力を進める(重力アキュムレータ)。level==0 は自然落下しない。
        /// 累積時間が落下間隔以上になるごとに 1 セルぶんの重力落下(移動または固定)を発火する。
        /// </summary>
        public void Advance(TimeSpan delta)
        {
            if (IsGameOver)
            {
                return;
            }

            if (Level == 0)
            {
                _gravityAccumulatorMs = 0;
                return;
            }

            _gravityAccumulatorMs += delta.TotalMilliseconds;
            while (_gravityAccumulatorMs >= GravityIntervalMs)
            {
                _gravityAccumulatorMs -= GravityIntervalMs;

                var cmd = new MoveDownCommand();
                var canAction = cmd.CanExecute(this);
                if (canAction)
                {
                    cmd.Execute(this);
                }

                UpdateGameState(GameCommand.moveDown, canAction);

                if (IsGameOver)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 現在の状態の読み取り専用スナップショットを生成する。
        /// </summary>
        public GameStateSnapshot Snapshot()
        {
            return new GameStateSnapshot(
                Field.Width,
                Field.Height,
                IsGameOver,
                Score,
                Level,
                LastTSpin,
                LastClearedLines,
                CurrentBlock.BlockType,
                CurrentBlock.GetBlockPoints(),
                ComputeGhostBlockPoints(),
                Field.GetFieldBlockPointAndTypePairs(),
                Field.GetFieldBlockPoints(),
                Field.GetFixedBlockPoints(),
                HoldBlock.BlockType,
                _blocksPoolManager.GetNextBlocksPool().Select(b => b.BlockType).ToList());
        }

        /// <summary>
        /// ゴーストブロック(現在ブロックをハードドロップした位置)の座標を算出する。
        /// フィールドは変更しない(旧 GameManager.GetGhostBlockPoints 相当)。
        /// </summary>
        public List<Position> ComputeGhostBlockPoints()
        {
            var ghost = new GameSession(Field, _blocksPoolManager);
            ghost.CurrentBlock = new Block(CurrentBlock);
            new HardDropCommand().Execute(ghost);
            return ghost.CurrentBlock.GetBlockPoints();
        }

        private bool CanSpawn()
        {
            return !Field.ExistsCollisionPoint(CurrentBlock);
        }

        private void UpdateGameState(GameCommand actType, bool canAction)
        {
            if (actType == GameCommand.hold)
            {
                if (CurrentBlock.BlockType == BlockTypes.nothing)
                {
                    CurrentBlock = _blocksPoolManager.TakeNextBlock();
                }
                if (!CanSpawn())
                {
                    IsGameOver = true;
                }
                _ = Field.UpdateField(CurrentBlock, false);
                _gravityAccumulatorMs = 0;
            }
            else
            {
                var isFixedBlock = NeedBlockFixed(actType, canAction);
                var line = Field.UpdateField(CurrentBlock, isFixedBlock);
                if (isFixedBlock)
                {
                    if (_beforeLine > 0)
                    {
                        _ren++;
                    }
                    else
                    {
                        _ren = 1;
                    }

                    var btb = _ren > 1 && _beforeT != TSpinTypes.notTSpin && CurrentBlock.TSpinType != TSpinTypes.notTSpin;
                    _scoreManager.Add(line, CurrentBlock.TSpinType, _ren, btb, Field.GetFixedBlockPoints().Count == 0);
                    _beforeLine = line;
                    _beforeT = CurrentBlock.TSpinType;
                    CurrentBlock = _blocksPoolManager.TakeNextBlock();
                    if (!CanSpawn())
                    {
                        IsGameOver = true;
                    }
                    _ = Field.UpdateField(CurrentBlock, false);
                    HoldBlock.CanSwap = true;
                    _gravityAccumulatorMs = 0;
                    UpdateGamelevel();
                }
                else
                {
                    _ren = 0;
                }
            }
        }

        private void UpdateGamelevel()
        {
            _scoreDelta += _scoreManager.Score - _beforeScore;

            if (_scoreDelta >= 500)
            {
                Level++;
                _scoreDelta = 0;
            }

            _beforeScore = _scoreManager.Score;
        }

        /// <summary>
        /// ブロックを固定するかを判定する。
        /// 下移動操作ができない時、ハードドロップした時にブロックを固定する(旧 NeedBlockFixed と同値)。
        /// </summary>
        private bool NeedBlockFixed(GameCommand actType, bool canAction)
        {
            if ((actType == GameCommand.moveDown && !canAction) || actType == GameCommand.hardDrop)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
