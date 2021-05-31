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

    public class Block
    {
        public BlockTypes BlockType { get; private set; }
        public Point Location { get { return _location; } }
        private int[,] _block;
        private readonly int _blockWidth;
        private readonly int _blockHeight;
        private Point _location = new Point();
        public Block(BlockTypes bt)
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
            _blockWidth = _block.GetLength(0);
            _blockHeight = _block.GetLength(1);
            _location = BlockType == BlockTypes.I ? new Point(3, -1) : new Point(3, 0);
        }

        public Block(Block b)
        {
            BlockType = b.BlockType;
            _block = new int[b._blockWidth, b._blockHeight];
            _blockWidth = b._blockWidth;
            _blockHeight = b._blockHeight;
            for (var x = 0; x < _blockWidth; x++)
            {
                for (var y = 0; y < _blockHeight; y++)
                {
                    _block[x, y] = b._block[x, y];
                }
            }
            _location = new Point(b._location.X, b._location.Y);
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
            var tmp = new int[_blockHeight, _blockWidth];
            for (var x = 0; x < _blockWidth;x++)
            {
                for (var y = 0; y < _blockHeight; y++)
                {
                    tmp[y, _blockWidth - x - 1] = _block[x, y];
                }
            }

            _block = tmp;
        }

        public void RotateLeft()
        {
            var tmp = new int[_blockHeight, _blockWidth];
            for (var x = 0; x < _blockWidth; x++)
            {
                for (var y = 0; y < _blockHeight; y++)
                {
                    tmp[_blockHeight- y - 1, x] = _block[x, y];
                }
            }

            _block = tmp;
        }

        public List<Point> GetBlockPoints()
        {
            var points = new List<Point>();
            for (var y = 0; y < _blockHeight; y++)
            {
                for (int x = 0; x < _blockWidth; x++)
                {
                    if (_block[x, y] == 1)
                    {
                        points.Add(new Point(x + _location.X, y + _location.Y));
                    }
                }
            }

            return points;
        }

        public List<Point> GetBlockBottomPoints()
        {
            var points = new List<Point>();
            for (int x = 0; x < _blockWidth; x++) 
            {
                for (var y = _blockHeight - 1; y >= 0; y--)
                {
                    if (_block[x, y] == 1)
                    {
                        points.Add(new Point(x + _location.X, y + _location.Y));
                        break;
                    }
                }
            }

            return points;
        }

        public List<Point> GetBlockLeftPoints()
        {
            var points = new List<Point>();
            for (var y = 0; y < _blockHeight; y++) 
            {
                for (int x = 0; x < _blockWidth; x++)
                {
                    if (_block[x, y] == 1)
                    {
                        points.Add(new Point(x + _location.X, y + _location.Y));
                        break;
                    }
                }
            }

            return points;
        }

        public List<Point> GetBlockRightPoints()
        {
            var points = new List<Point>();
            for (var y = 0; y < _blockHeight; y++) 
            {
                for (int x = _blockWidth - 1; x >= 0; x--)
                {
                    if (_block[x, y] == 1)
                    {
                        points.Add(new Point(x + _location.X, y + _location.Y));
                        break;
                    }
                }
            }

            return points;
        }

        public List<Point> GetBlockRotatePoints()
        {
            var points = new List<Point>();
            for (var x = 0; x < _blockWidth; x++)
            {
                for (var y = 0; y < _blockHeight; y++)
                {
                    if (_block[x, y] == 1)
                    {
                        if(BlockType == BlockTypes.O || BlockType == BlockTypes.I)
                        {
                            points.Add(new Point(x + _location.X, y + _location.Y));
                        }
                        else
                        {
                            if(!(x == 1 && y == 1))
                            {
                                points.Add(new Point(x + _location.X, y + _location.Y));
                            }
                        }
                    }
                }
            }

            return points;
        }

        public Point GetLocation_BlockTop()
        {
            for (var y = 0; y < _blockHeight; y++) 
            {
                for (int x = 0; x < _blockWidth; x++)
                {
                    if (_block[x, y] == 1)
                    {
                        return new Point(x + _location.X, y + _location.Y);
                    }
                }
            }

            return new Point(-1, -1);
        }

        public Point GetGetLocation_BlockBottom()
        {
            for (var y = _blockHeight - 1; y >= 0; y--) 
            {
                for (int x = 0; x < _blockWidth; x++)
                {
                    if (_block[x, y] == 1)
                    {
                        return new Point(x + _location.X, y + _location.Y);
                    }
                }
            }

            return new Point(-1, -1);
        }

        public Point GetLocation_BlockLeft()
        {
            for (int x = 0; x < _blockWidth; x++) 
            {
                for (var y = 0; y < _blockHeight; y++)
                {
                    if (_block[x, y] == 1)
                    {
                        return new Point(x + _location.X, y + _location.Y);
                    }
                }
            }

            return new Point(-1, -1);
        }

        public Point GetLocation_BlockRight()
        {
            for (int x = _blockWidth - 1; x >= 0; x--) 
            {
                for (var y = 0; y < _blockHeight; y++)
                {
                    if (_block[x, y] == 1)
                    {
                        return new Point(x + _location.X, y + _location.Y);
                    }
                }
            }

            return new Point(-1, -1);
        }

        /// <summary>
        /// ■■ 
        /// ■■ 
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBlockO()
        {
            var rect = new int[2, 2];
            rect[0, 0] = 1;
            rect[0, 1] = 1;
            rect[1, 0] = 1;
            rect[1, 1] = 1;
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
            var rect = new int[4, 4];
            rect[0, 1] = 1;
            rect[1, 1] = 1;
            rect[2, 1] = 1;
            rect[3, 1] = 1;
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
            var rect = new int[3, 3];
            rect[1, 0] = 1;
            rect[0, 1] = 1;
            rect[1, 1] = 1;
            rect[2, 1] = 1;
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
            var rect = new int[3, 3];
            rect[0, 0] = 1;
            rect[0, 1] = 1;
            rect[1, 1] = 1;
            rect[2, 1] = 1;
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
            var rect = new int[3, 3];
            rect[2, 0] = 1;
            rect[0, 1] = 1;
            rect[1, 1] = 1;
            rect[2, 1] = 1;
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
            var rect = new int[3, 3];
            rect[0, 0] = 1;
            rect[1, 0] = 1;
            rect[1, 1] = 1;
            rect[2, 1] = 1;
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
            var rect = new int[3, 3];
            rect[1, 0] = 1;
            rect[2, 0] = 1;
            rect[0, 1] = 1;
            rect[1, 1] = 1;
            return rect;
        }
    }
}
