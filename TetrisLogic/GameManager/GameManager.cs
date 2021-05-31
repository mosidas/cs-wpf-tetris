using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using TetrisLogic.UserAction;

namespace TetrisLogic
{
    public class GameManager
    {
        public bool IsGameOver { get; set; }
        public double FrameRate { get { return 1000 / FPS; } }
        public double GameLevel { get; private set; }
        public double DownRate { get { return FrameRate * (50 / GameLevel * 5); } }
        public List<Point> CurrentBlockPoints { get { return _currentBlock == null ? new List<Point>() : _currentBlock.GetBlockPoints(); } }
        public BlockTypes CurrentBlockType { get { return _currentBlock == null ? BlockTypes.nothing : _currentBlock.BlockType; } }
        public List<Point> FixedBlockPoints { get { return _field == null ? new List<Point>() : _field.GetFixedBlockPoints(); } }
        public List<BlockTypes> FixedBlockTypes { get { return _field == null ? new List<BlockTypes>() : _field.GetFixedBlockTypes(); } }
        public List<(Point, BlockTypes)> FieldPointAndTypePairs { get { return _field == null ? new List<(Point, BlockTypes)>() : _field.GetFieldBlockPointAndTypePairs(); } }
        public List<Point> FieldBlockPoints { get { return _field == null ? new List<Point>() : _field.GetFieldBlockPoints(); } }
        public int FieldWidth { get { return _field == null ? 0 : _field.Width; } }
        public int FieldHeight { get { return _field == null ? 0 : _field.Height; } }

        public static readonly int FPS = 60;
        private Field _field;
        private Block _currentBlock;
        private Block _holdBlock;
        private readonly IBlocksPoolManager _blocksPoolManager;

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
            _field.InitField();
            _blocksPoolManager.Reset();
            _currentBlock = _blocksPoolManager.TakeNextBlock();
            _holdBlock = null;
            _field.UpdateField(_currentBlock, false);
        }

        public void Update(ActionTypes userAction, bool doTimerAction)
        {
            if (IsGameOver)
            {
                GameOver();
                return;
            }

            if (doTimerAction)
            {
                var canTimerAction = Act(ActionTypes.moveDown);
                UpdateGameState(ActionTypes.moveDown, canTimerAction);
            }

            if(DoContinueSameAction(userAction))
            {
                var canUserAction = Act(userAction);
                UpdateGameState(userAction, canUserAction);
            }
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
                if (_actionCount % 10 == 0 || _actionCount > 100)
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

        private bool Act(ActionTypes actType)
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

        private void UpdateGameState(ActionTypes actType, bool canAction)
        {
            var isFixedBlock = NeedBlockFixed(actType, canAction);
            _field.UpdateField(_currentBlock, isFixedBlock);
            if (isFixedBlock)
            {
                _currentBlock = _blocksPoolManager.TakeNextBlock();
                if (!CanSpawn())
                {
                    IsGameOver = true;
                }
                _field.UpdateField(_currentBlock, false);
            }
        }

        /// <summary>
        /// ブロックを固定するかを判定する。
        /// 時間経過の落下ができない時、下移動操作ができない時、ハードドロップした時にブロックを固定する。
        /// </summary>
        /// <param name="actType">操作タイプ</param>
        /// <param name="canAction">操作の実行可否</param>
        /// <returns>判定結果</returns>
        private bool NeedBlockFixed(ActionTypes actType, bool canAction)
        {
            if ((actType == ActionTypes.moveDown && !canAction) || actType == ActionTypes.hardDrop)
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
