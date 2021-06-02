using System;
using System.Collections.Generic;
using System.Drawing;
using TetrisLogic.UserAction;

namespace TetrisLogic
{
    public class GameManager
    {
        public bool IsGameOver { get; set; }
        public double FrameRate { get { return 1000 / FPS; } }
        public double GameLevel { get; private set; }
        public double DownRate { get { return GameLevel == 0 ? 0 : FPS * (10 / Math.Floor(Math.Log2(GameLevel + 1))); } }
        public List<Point> CurrentBlockPoints { get { return _currentBlock == null ? new List<Point>() : _currentBlock.GetBlockPoints(); } }
        public List<Point> FixedBlockPoints { get { return _field == null ? new List<Point>() : _field.GetFixedBlockPoints(); } }
        public List<(Point, BlockTypes)> FieldPointAndTypePairs { get { return _field == null ? new List<(Point, BlockTypes)>() : _field.GetFieldBlockPointAndTypePairs(); } }
        public List<Point> FieldBlockPoints { get { return _field == null ? new List<Point>() : _field.GetFieldBlockPoints(); } }
        public Block HoldBlock { get { return _holdBlock; } }
        public int FieldWidth { get { return _field == null ? 0 : _field.Width; } }
        public int FieldHeight { get { return _field == null ? 0 : _field.Height; } }

        public static readonly int FPS = 60;
        private Field _field;
        private Block _currentBlock;
        private Block _holdBlock;
        private readonly IBlocksPoolManager _blocksPoolManager;
        private int _timeCounter;

        public GameManager(Field field, IBlocksPoolManager bpm)
        {
            _field = field;
            _blocksPoolManager = bpm;
            IsGameOver = true;
        }

        public void Start(int gamelevel = 5)
        {
            GameLevel = Math.Min(gamelevel,1024);
            IsGameOver = false;
            _field.InitField();
            _blocksPoolManager.Reset();
            _currentBlock = _blocksPoolManager.TakeNextBlock();
            _holdBlock = new Block(BlockTypes.nothing);
            _field.UpdateField(_currentBlock, false);
            _timeCounter = 0;
        }

        public void Update(ActionTypes userAction)
        {
            if (IsGameOver)
            {
                return;
            }

            if(GameLevel == 0)
            {
                _timeCounter = 0;
            }
            else
            {
                if (_timeCounter >= DownRate)
                {
                    var canTimerAction = Act(ActionTypes.moveDown);
                    UpdateGameState(ActionTypes.moveDown, canTimerAction);
                    _timeCounter = 0;
                }
                else
                {
                    _timeCounter += 1;
                }
            }


            if(DoContinueSameAction(userAction))
            {
                var canUserAction = Act(userAction);
                UpdateGameState(userAction, canUserAction);
            }
        }

        public void DegubWrite()
        {
            System.Diagnostics.Debug.WriteLine("--current block--");
            System.Diagnostics.Debug.WriteLine(_currentBlock.DrawBlock());
            System.Diagnostics.Debug.WriteLine("--current hold block--");
            System.Diagnostics.Debug.WriteLine(_holdBlock.DrawBlock());
            System.Diagnostics.Debug.WriteLine("--current filed--");
            System.Diagnostics.Debug.WriteLine(_field.DrawFiled());
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

        private bool CanSpawn()
        {
            return !_field.ExistsCollisionPoint(_currentBlock);
        }

        private bool Act(ActionTypes actType)
        {
            var factory = new UserActionFactory();
            var userAction = factory.CreateUserAction(actType);
            if(userAction == null)
            {
                return true;
            }

            var canAction = userAction.CanAction(_field, _currentBlock, _holdBlock);
            if(canAction)
            {
                userAction.Action(ref _field, ref _currentBlock, ref _holdBlock);
            }

            return canAction;
        }

        private void UpdateGameState(ActionTypes actType, bool canAction)
        {
            if(actType == ActionTypes.hold)
            {
                if(_currentBlock.BlockType == BlockTypes.nothing)
                {
                    _currentBlock = _blocksPoolManager.TakeNextBlock();
                }
                if (!CanSpawn())
                {
                    IsGameOver = true;
                }
                _ = _field.UpdateField(_currentBlock, false);
                _timeCounter = 0;
            }
            else
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
                    _ = _field.UpdateField(_currentBlock, false);
                    _holdBlock.CanSwap = true;
                    _timeCounter = 0;
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
