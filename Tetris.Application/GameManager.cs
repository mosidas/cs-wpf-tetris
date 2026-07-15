using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Domain;

namespace Tetris.Application
{
    /// <summary>
    /// 旧 GameManager の公開契約を保つ薄い互換ファサード。GameSession へ委譲し、
    /// 新規に Tick/Snapshot を公開する。WPF と GameManagerTests の差分を最小化する。
    /// </summary>
    public sealed class GameManager
    {
        private readonly GameSession _session;
        private readonly IBlocksPoolManager _blocksPoolManager;

        public GameManager(Field field, IBlocksPoolManager pool)
        {
            _session = new GameSession(field, pool);
            _blocksPoolManager = pool;
        }

        public bool IsGameOver { get { return _session.IsGameOver; } }

        public int Score { get { return _session.Score; } }

        public TSpinTypes TSpinType { get { return _session.LastTSpin; } }

        public int Line { get { return _session.LastClearedLines; } }

        /// <summary>
        /// 1 フレームの長さ(単位:ms)。WPF タイマ間隔用。
        /// </summary>
        public double FrameRate { get { return 1000.0 / GameSession.FPS; } }

        /// <summary>
        /// ゲームレベル
        /// </summary>
        public double GameLevel { get { return _session.Level; } }

        /// <summary>
        /// 自然に落下するスピード(単位:フレーム)。ゲームレベルによって決まる(旧 DownRate)。
        /// </summary>
        public double DownRate
        {
            get { return _session.Level == 0 ? 0 : Math.Max(1, GameSession.FPS - _session.Level * 5); }
        }

        public List<Position> CurrentBlockPoints
        {
            get { return _session.CurrentBlock.GetBlockPoints(); }
        }

        public BlockTypes CurrentBlocktype { get { return _session.CurrentBlock.BlockType; } }

        public List<Position> GhostBlockPoints { get { return _session.ComputeGhostBlockPoints(); } }

        public List<Position> FixedBlockPoints { get { return _session.Field.GetFixedBlockPoints(); } }

        public List<(Position, BlockTypes)> FieldPointAndTypePairs
        {
            get { return _session.Field.GetFieldBlockPointAndTypePairs(); }
        }

        public List<Position> FieldBlockPoints { get { return _session.Field.GetFieldBlockPoints(); } }

        public BlockTypes HoldBlockType { get { return _session.HoldBlock.BlockType; } }

        public List<BlockTypes> NextBlockTypes
        {
            get { return _blocksPoolManager.GetNextBlocksPool().Select(b => b.BlockType).ToList(); }
        }

        public int FieldWidth { get { return _session.Field.Width; } }

        public int FieldHeight { get { return _session.Field.Height; } }

        /// <summary>
        /// ゲームを開始する。ゲームレベルは 0 - 99。ゲームレベル 0 だと自然落下しない。
        /// </summary>
        public void Start(int gamelevel = 1)
        {
            _session.Start(gamelevel);
        }

        /// <summary>
        /// 旧フレーム駆動の更新。1 回 = 1 フレーム相当(FrameRate ms ぶんの重力 + 入力連射制御 + コマンド適用)。
        /// </summary>
        public void Update(ActionTypes userAction)
        {
            if (_session.IsGameOver)
            {
                return;
            }

            _session.Advance(TimeSpan.FromMilliseconds(FrameRate));

            if (DoContinueSameAction(userAction))
            {
                _session.Apply(Map(userAction));
            }
        }

        /// <summary>
        /// 経過時間ぶんゲームを進める(GameSession.Advance へ委譲)。
        /// </summary>
        public void Tick(TimeSpan delta)
        {
            _session.Advance(delta);
        }

        /// <summary>
        /// 現在の状態の読み取り専用スナップショットを返す。
        /// </summary>
        public GameStateSnapshot Snapshot()
        {
            return _session.Snapshot();
        }

        private ActionTypes _beforeAction = ActionTypes.nothing;
        private int _actionCount;
        private bool DoContinueSameAction(ActionTypes userAction)
        {
            var bAct = _beforeAction;
            _beforeAction = userAction;

            if (userAction == ActionTypes.nothing)
            {
                _actionCount = 0;
                return false;
            }

            if (bAct == userAction)
            {
                _actionCount++;
                if (_actionCount >= 25 && _actionCount % 3 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                _actionCount = 0;
                return true;
            }
        }

        private static GameCommand Map(ActionTypes userAction)
        {
            return userAction switch
            {
                ActionTypes.moveLeft => GameCommand.moveLeft,
                ActionTypes.moveRight => GameCommand.moveRight,
                ActionTypes.moveDown => GameCommand.moveDown,
                ActionTypes.rotateLeft => GameCommand.rotateLeft,
                ActionTypes.rotateRight => GameCommand.rotateRight,
                ActionTypes.hold => GameCommand.hold,
                ActionTypes.hardDrop => GameCommand.hardDrop,
                _ => GameCommand.nothing,
            };
        }
    }
}
