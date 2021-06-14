using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TetrisLogic.UserAction;

namespace TetrisLogic
{
    /// <summary>
    /// ゲームの動きを表す。
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// true:ゲーム終了、false:ゲーム中
        /// </summary>
        public bool IsGameOver { get; set; }

        public int Score { get { return _scoreManager.Score; } }

        public TSpinTypes TSpinType { get { return _beforeT; } }

        public int Line { get { return _beforeLine; } }
        /// <summary>
        /// 1フレームの長さ(単位:ms)
        /// </summary>
        public double FrameRate { get { return 1000 / FPS; } }
        /// <summary>
        /// ゲームレベル
        /// </summary>
        public double GameLevel { get; private set; }
        /// <summary>
        /// 自然に落下するスピード(単位:ms) ゲームレベルによって決まる
        /// </summary>
        public double DownRate { get { return GameLevel == 0 ? 0 : FPS * (10 / Math.Floor(Math.Log2(GameLevel + 1))); } }
        /// <summary>
        /// 操作中のブロックの座標
        /// </summary>
        public List<Point> CurrentBlockPoints { get { return _currentBlock == null ? new List<Point>() : _currentBlock.GetBlockPoints(); } }
        /// <summary>
        /// 操作中のブロックのブロックタイプ
        /// </summary>
        public BlockTypes CurrentBlocktype { get { return _currentBlock.BlockType; } }
        /// <summary>
        /// ゴーストブロックの座標
        /// </summary>
        public List<Point> GhostBlockPoints { get { return GetGhostBlockPoints(); } }
        /// <summary>
        /// すべての固定ブロックの座標
        /// </summary>
        public List<Point> FixedBlockPoints { get { return _field == null ? new List<Point>() : _field.GetFixedBlockPoints(); } }
        /// <summary>
        /// すべてのフィールド上にあるブロック(操作中のブロック + 固定ブロック)の座標とそのブロックタイプ
        /// </summary>
        public List<(Point, BlockTypes)> FieldPointAndTypePairs { get { return _field == null ? new List<(Point, BlockTypes)>() : _field.GetFieldBlockPointAndTypePairs(); } }
        /// <summary>
        /// すべてのフィールド上にあるブロック(操作中のブロック + 固定ブロック)の座標
        /// </summary>
        public List<Point> FieldBlockPoints { get { return _field == null ? new List<Point>() : _field.GetFieldBlockPoints(); } }
        /// <summary>
        /// ホールドブロックのブロックタイプ ホールドブロックがなければBlockType.nothig
        /// </summary>
        public BlockTypes HoldBlockType { get { return _holdBlock.BlockType; } }
        /// <summary>
        /// 次のブロック プールにあるブロックの落ちてくる順
        /// </summary>
        public List<BlockTypes> NextBlockTypes { get { return _blocksPoolManager.GetNextBlocksPool().Select(b => b.BlockType).ToList(); } }
        /// <summary>
        /// フィールドの幅
        /// </summary>
        public int FieldWidth { get { return _field == null ? 0 : _field.Width; } }
        /// <summary>
        /// フィールドの高さ
        /// </summary>
        public int FieldHeight { get { return _field == null ? 0 : _field.Height; } }
        /// <summary>
        /// FPS
        /// </summary>
        public static readonly int FPS = 60;

        private Field _field;
        private Block _currentBlock;
        private Block _holdBlock;
        private readonly IBlocksPoolManager _blocksPoolManager;
        private ScoreManager _scoreManager;
        private int _timeCounter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameManager(Field field, IBlocksPoolManager bpm)
        {
            _field = field;
            _currentBlock = new Block(BlockTypes.nothing);
            _holdBlock = new Block(BlockTypes.nothing);
            _blocksPoolManager = bpm;
            _scoreManager = new ScoreManager();
            IsGameOver = true;
        }

        /// <summary>
        /// ゲームを開始する ゲームレベルは0 - 1024 ゲームレベル0だと自然落下しない
        /// </summary>
        public void Start(int gamelevel = 5)
        {
            GameLevel = Math.Min(gamelevel,1024);
            IsGameOver = false;
            _field.InitField();
            _blocksPoolManager.Reset();
            _currentBlock = _blocksPoolManager.TakeNextBlock();
            _holdBlock = new Block(BlockTypes.nothing);
            _field.UpdateField(_currentBlock, false);
            _scoreManager.Reset();
            _timeCounter = 0;
        }

        /// <summary>
        /// ユーザーの操作に応じてゲーム状態(各プロパティ)を更新する
        /// </summary>
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

        /// <summary>
        /// デバッグ用
        /// </summary>
        public void DegubWrite()
        {
            System.Diagnostics.Debug.WriteLine("--current block--");
            System.Diagnostics.Debug.WriteLine(_currentBlock.DrawBlock());
            System.Diagnostics.Debug.WriteLine("--current hold block--");
            System.Diagnostics.Debug.WriteLine(_holdBlock.DrawBlock());
            System.Diagnostics.Debug.WriteLine("--current filed--");
            System.Diagnostics.Debug.WriteLine(_field.DrawField());
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

        private int _ren;
        private int _beforeLine;
        private TSpinTypes _beforeT;

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
                var line = _field.UpdateField(_currentBlock, isFixedBlock);
                if (isFixedBlock)
                {
                    if(_beforeLine > 0)
                    {
                        _ren++;
                    }
                    else
                    {
                        _ren = 1;
                    }

                    var btb = _ren > 1 && _beforeT != TSpinTypes.notTSpin && _currentBlock.TSpinType != TSpinTypes.notTSpin;
                    _scoreManager.Add(line, _currentBlock.TSpinType, _ren, btb, !FixedBlockPoints.Any());
                    _beforeLine = line;
                    _beforeT = _currentBlock.TSpinType;
                    _currentBlock = _blocksPoolManager.TakeNextBlock();
                    if (!CanSpawn())
                    {
                        IsGameOver = true;
                    }
                    _ = _field.UpdateField(_currentBlock, false);
                    _holdBlock.CanSwap = true;
                    _timeCounter = 0;
                }
                else
                {
                    _ren = 0;
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

        private List<Point> GetGhostBlockPoints()
        {
            var hd = new UA_HardDrop();
            var ghostBlock = new Block(_currentBlock);
            var dm = new Block(BlockTypes.nothing);
            hd.Action(ref _field, ref ghostBlock, ref dm);
            return ghostBlock.GetBlockPoints();
        }
    }
}
