using System.Collections.Generic;
using System.Drawing;

namespace TetrisLogic
{
    /// <summary>
    /// ブロックのタイプ
    /// </summary>
    public enum BlockTypes
    {
        nothing,
        T,
        I,
        J,
        L,
        S,
        Z,
        O,
    }

    /// <summary>
    /// ブロックの向き
    /// </summary>
    public enum DirectionTypes
    {
        north = 0,
        east,
        south,
        west,
    }

    public enum TSpinTypes
    {
        notTSpin,
        tMini,
        tSpin,
    }

    /// <summary>
    /// ブロックのクラス
    /// </summary>
    public class Block
    {
        /// <summary>
        /// ブロックのタイプ
        /// </summary>
        public BlockTypes BlockType { get; private set; }
        /// <summary>
        /// ブロックの位置
        /// </summary>
        public Point Location { get { return _location; } }
        /// <summary>
        /// ブロックの向き
        /// </summary>
        public DirectionTypes Direction { get { return _direction; } }
        /// <summary>
        /// ホールドブロックが交換可能か
        /// </summary>
        public bool CanSwap { get; set; }

        public TSpinTypes TSpinType { get; set; }

        private int[,] _block;
        private readonly int _blockWidth;
        private readonly int _blockHeight;
        private Point _location;
        private DirectionTypes _direction;
        /// <summary>
        /// コンストラクタ ブロックタイプと交換可能か(ホールドブロックの時)を指定する
        /// </summary>
        public Block(BlockTypes bt, bool canSwap = true)
        {
            BlockType = bt;
            switch(bt)
            {
                case BlockTypes.O:
                    _block = CreateBlockO();
                    break;
                case BlockTypes.I:
                    _block = CreateBlockI();
                    break;
                case BlockTypes.T:
                    _block = CreateBlockT();
                    break;
                case BlockTypes.J:
                    _block = CreateBlockJ();
                    break;
                case BlockTypes.L:
                    _block = CreateBlockL();
                    break;
                case BlockTypes.Z:
                    _block = CreateBlockZ();
                    break;
                case BlockTypes.S:
                    _block = CreateBlockS();
                    break;
                default:
                    _block = new int[1, 1];
                    break;
            }
            _blockWidth =  _block.GetLength(1);
            _blockHeight = _block.GetLength(0);
            _location = BlockType == BlockTypes.I ? new Point(3, -1) : new Point(3, 0);
            _direction = DirectionTypes.north;
            CanSwap = canSwap;
            TSpinType = TSpinTypes.notTSpin;
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        public Block(Block b)
        {
            BlockType = b.BlockType;
            _block = new int[b._blockHeight, b._blockWidth];
            _blockWidth = b._blockWidth;
            _blockHeight = b._blockHeight;
            for (var row = 0; row < _blockHeight; row++)
            {
                for (var col = 0; col < _blockWidth; col++)
                {
                    _block[row, col] = b._block[row, col];
                }
            }
            _location = new Point(b._location.X, b._location.Y);
            _direction = b._direction;
            TSpinType = b.TSpinType;
        }

        /// <summary>
        /// 位置を初期位置に戻す
        /// </summary>
        public void ResetLocation()
        {
            _location = BlockType == BlockTypes.I ? new Point(3, -1) : new Point(3, 0);
        }

        /// <summary>
        /// 位置を移動する
        /// </summary>
        public void MoveLocation(int x,int y)
        {
            _location = new Point(_location.X + x, _location.Y + y);
        }

        /// <summary>
        /// 右回転する north -> east -> south -> west -> north
        /// </summary>
        public void RotateRight()
        {
            var tmp = new int[_blockWidth, _blockHeight];
            for (var row = 0; row < _blockHeight; row++)
            {
                for (var col = 0; col < _blockWidth; col++)
                {
                    tmp[col, _blockHeight - row - 1] = _block[row, col];
                }
            }
            _block = tmp;
            _direction = (DirectionTypes)(((int)_direction + 1) % 4);
        }

        /// <summary>
        /// 左回転する north -> west -> south -> east -> north
        /// </summary>
        public void RotateLeft()
        {
            var tmp = new int[_blockWidth, _blockHeight];
            for (var row = 0; row < _blockHeight; row++)
            {
                for (var col = 0; col < _blockWidth; col++)
                {
                    tmp[_blockWidth - col - 1, row] = _block[row, col];
                }
            }

            _block = tmp;
            _direction = (DirectionTypes)(((int)_direction + 3) % 4);
        }

        /// <summary>
        ///■□■
        ///□□□
        ///■□■
        /// </summary>
        /// <returns></returns>
        public List<Point> GetTSpinPoints()
        {
            if(BlockType != BlockTypes.T)
            {
                return new List<Point>();
            }

            var points = new List<Point>
            {
                new Point(0 + _location.X, 0 + _location.Y),
                new Point(2 + _location.X, 0 + _location.Y),
                new Point(0 + _location.X, 2 + _location.Y),
                new Point(2 + _location.X, 2 + _location.Y)
            };

            return points;
        }

        /// <summary>
        /// ブロックのフィールド上の座標を取得する
        /// </summary>
        public List<Point> GetBlockPoints()
        {
            var points = new List<Point>();
            for (var row = 0; row < _blockHeight; row++)
            {
                for (var col = 0; col < _blockWidth; col++)
                {
                    if (_block[row, col] == 1)
                    {
                        points.Add(new Point(col + _location.X, row + _location.Y));
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// ブロック下面のフィールド上の座標を取得する
        /// </summary>
        public List<Point> GetBlockBottomPoints()
        {
            var points = new List<Point>();
            for (int col = 0; col < _blockWidth; col++)
            {
                for (int row = _blockHeight - 1; row >= 0; row--)
                {
                    if (_block[row, col] == 1)
                    {
                        points.Add(new Point(col + _location.X, row + _location.Y));
                        break;
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// ブロック左面のフィールド上の座標を取得する
        /// </summary>
        public List<Point> GetBlockLeftPoints()
        {
            var points = new List<Point>();
            for (var row = 0; row < _blockHeight; row++) 
            {
                for (int col = 0; col < _blockWidth; col++)
                {
                    if (_block[row, col] == 1)
                    {
                        points.Add(new Point(col + _location.X, row + _location.Y));
                        break;
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// ブロック右面のフィールド上の座標を取得する
        /// </summary>
        public List<Point> GetBlockRightPoints()
        {
            var points = new List<Point>();
            for (var row = 0; row < _blockHeight; row++) 
            {
                for (int col = _blockWidth - 1; col >= 0; col--)
                {
                    if (_block[row, col] == 1)
                    {
                        points.Add(new Point(col + _location.X, row + _location.Y));
                        break;
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// ブロックの回転軸のブロック以外のフィールド上の座標を取得する
        /// </summary>
        public List<Point> GetBlockRotatePoints()
        {
            var points = new List<Point>();
            for (var col = 0; col < _blockWidth; col++)
            {
                for (var row = 0; row < _blockHeight; row++)
                {
                    if (_block[row, col] == 1)
                    {
                        if(BlockType == BlockTypes.O || BlockType == BlockTypes.I)
                        {
                            points.Add(new Point(col + _location.X, row + _location.Y));
                        }
                        else
                        {
                            if(!(col == 1 && row == 1))
                            {
                                points.Add(new Point(col + _location.X, row + _location.Y));
                            }
                        }
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// ブロックの内部構造を文字列に変化して取得する(デバッグ用)
        /// </summary>
        public string DrawBlock()
        {
            var ret = new List<string>();

            for (int i = 0; i < _block.GetLength(0); i++)
            {
                var tmp = new List<string>();
                for (int j = 0; j < _block.GetLength(1); j++)
                {
                    tmp.Add(_block[i, j].ToString());
                }

                ret.Add(string.Join(",", tmp));
            }

            var rret = string.Join("\r\n", ret);

            return rret;
        }

        /// <summary>
        /// ■■ 
        /// ■■ 
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockO()
        {
            var rect = new int[2, 2]
            {            
                { 1, 1, },
                { 1, 1, },
            };
            return rect;
        }

        /// <summary>
        /// □□□□
        /// ■■■■
        /// □□□□
        /// □□□□
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockI()
        {
            var rect = new int[4, 4]
            {
                { 0, 0, 0, 0, },
                { 1, 1, 1, 1, },
                { 0, 0, 0, 0, },
                { 0, 0, 0, 0, },
            };
            return rect;
        }

        /// <summary>
        ///□■□
        ///■■■
        ///□□□
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockT()
        {
            var rect = new int[3, 3]
            {           
                { 0, 1, 0, },
                { 1, 1, 1, },
                { 0, 0, 0, },
            };

            return rect;
        }

        /// <summary>
        ///■□□
        ///■■■
        ///□□□
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockJ()
        {
            var rect = new int[3, 3]           
            {
                { 1, 0, 0, },
                { 1, 1, 1, },
                { 0, 0, 0, },
            };
            return rect;
        }

        /// <summary>
        ///□□■ 
        ///■■■
        ///□□□
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockL()
        {
            var rect = new int[3, 3]           
            {
                { 0, 0, 1, },
                { 1, 1, 1, },
                { 0, 0, 0, },
            };
            return rect;
        }

        /// <summary>
        ///■■□
        ///□■■ 
        ///□□□ 
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockZ()
        {
            var rect = new int[3, 3]
            {
                { 1, 1, 0, },
                { 0, 1, 1, },
                { 0, 0, 0, },
            };
            return rect;
        }

        /// <summary>
        ///□■■
        ///■■□
        ///□□□
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockS()
        {
            var rect = new int[3, 3]
            {
                { 0, 1, 1, },
                { 1, 1, 0, },
                { 0, 0, 0, },
            };
            return rect;
        }
    }
}
