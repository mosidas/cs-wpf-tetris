using System.Collections.Generic;
using System.Drawing;

namespace TetrisLogic
{
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

    public enum DirectionTypes
    {
        north = 0,
        east,
        south,
        west,
    }

    public class Block
    {
        public BlockTypes BlockType { get; private set; }
        public Point Location { get { return _location; } }

        public DirectionTypes Direction { get { return _direction; } }

        public bool CanSwap { get; set; }

        private int[,] _block;
        private readonly int _blockWidth;
        private readonly int _blockHeight;
        private Point _location;
        private DirectionTypes _direction;
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
        }

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
        }

        public void ResetLocation()
        {
            _location = BlockType == BlockTypes.I ? new Point(3, -1) : new Point(3, 0);
        }

        public void MoveLocation(int x,int y)
        {
            _location = new Point(_location.X + x, _location.Y + y);
        }

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
