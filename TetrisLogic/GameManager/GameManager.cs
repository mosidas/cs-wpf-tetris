using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using TetrisLogic.UserAction;
using static TetrisLogic.SystemProperty;

namespace TetrisLogic
{
    public class GameManager
    {
        public bool IsGameOver { get; set; }
        public bool IsSpawn { get; set; }
        public double FrameRate { get { return 1000 / SystemProperty.FPS; } }
        public double GameLevel { get; private set; }
        public double DownRate { get { return FrameRate * (50 / GameLevel * 5); } }

        public List<Point> CurrentBlockPoints { get { return _currentBlock.GetBlockPoints(); } }

        public BlockType CurrentBlockType { get { return _currentBlock.BlockType; } }

        public List<Point> FixedBlockPoints { get { return _field.GetFixedBlockPoints(); } }

        public List<BlockType> FixedBlockTypes { get { return _field.GetFixedBlockTypes(); } }

        private Field _field;
        private Block _currentBlock;
        private Block _holdBlock;
        private readonly IBlocksPoolManager _blocksPoolManager;
        private Timer TimersTimer;

        public GameManager(Field field, IBlocksPoolManager bpm)
        {
            _field = field;
            _blocksPoolManager = bpm;
            IsGameOver = true;
        }

        public void Start(int gamelevel = 5)
        {
            GameLevel = gamelevel;
            IsGameOver = false;
            _field.InitFieldState();
            _blocksPoolManager.Reset();
            _currentBlock = _blocksPoolManager.TakeNextBlock();
            _holdBlock = null;
        }

        private ActionType _beforeAction = ActionType.nothing;
        private int _actionCount = 0;

        public void Update(ActionType userAction, bool doTimerAction)
        {
            if (IsGameOver)
            {
                GameOver();
                return;
            }

            if (doTimerAction)
            {
                var canTimerAction = Act(ActionType.moveDown);
                UpdateState(ActionType.moveDown, canTimerAction);
            }

            if(DoContinueSameAction(userAction))
            {
                var canUserAction = Act(userAction);
                UpdateState(userAction, canUserAction);
            }
        }

        private bool DoContinueSameAction(ActionType userAction)
        {
            var bAct = _beforeAction;
            _beforeAction = userAction;

            if (userAction == ActionType.nothing)
            {
                _actionCount = 0;
                return false;
            }

            if (bAct == userAction)
            {
                _actionCount++;
                if (_actionCount % 10 == 0)
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

        private void GameOver()
        {
            
        }

        private bool CanSpawn()
        {
            return _field.CanSpawn(_currentBlock);
        }

        private bool Act(ActionType actType)
        {
            var factory = new UserActionFactory();
            var userAction = factory.CreateUserAction(actType);
            if(userAction == null)
            {
                return true;
            }

            var canAction = userAction.CanAction(_field, _currentBlock);
            if(canAction)
            {
                userAction.Action(ref _field, ref _currentBlock, ref _holdBlock);
            }

            return canAction;
        }

        private void UpdateState(ActionType actType, bool canAction)
        {
            var isFixedBlock = NeedBlockFixed(actType, canAction);
            _field.UpdateFieldState(_currentBlock, isFixedBlock);
            if (isFixedBlock)
            {
                _currentBlock = _blocksPoolManager.TakeNextBlock();
                if (!CanSpawn())
                {
                    IsGameOver = true;
                }
            }
        }

        /// <summary>
        /// ブロックを固定するかを判定する。
        /// 時間経過の落下ができない時、下移動操作ができない時、ハードドロップした時にブロックを固定する。
        /// </summary>
        /// <param name="actType">操作タイプ</param>
        /// <param name="canAction">操作の実行可否</param>
        /// <returns>判定結果</returns>
        private bool NeedBlockFixed(ActionType actType, bool canAction)
        {
            if ((actType == ActionType.moveDown && !canAction) || actType == ActionType.hardDrop)
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
