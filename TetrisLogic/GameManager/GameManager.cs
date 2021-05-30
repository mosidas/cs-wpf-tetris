using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly BlocksPoolManager _blocksPoolManager;

        public GameManager()
        {
            _field = new Field();
            _blocksPoolManager = new BlocksPoolManager();
            IsGameOver = true;
        }

        public void Start()
        {
            GameLevel = 5;
            IsGameOver = false;
            _field.InitFieldState();
            _blocksPoolManager.Reset();
            _currentBlock = _blocksPoolManager.GetNextBlock();
            _holdBlock = null;
        }

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

            var canUserAction = Act(userAction);
            UpdateState(userAction, canUserAction);
        }

        private void GameOver()
        {
            throw new NotImplementedException();
        }

        private bool CanSpawn()
        {
            return _field.CanSpawn();
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
                if (CanSpawn())
                {
                    _currentBlock = _blocksPoolManager.GetNextBlock();
                }
                else
                {
                    IsGameOver = true;
                }
            }
        }

        private bool NeedBlockFixed(ActionType actType, bool canAction)
        {
            if (actType == ActionType.moveDown && !canAction)
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
