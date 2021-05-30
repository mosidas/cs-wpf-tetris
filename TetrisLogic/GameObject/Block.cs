using System.Collections.Generic;
using System.Drawing;
using static TetrisLogic.SystemProperty;

namespace TetrisLogic
{
    public class Block
    {
        public BlockType BlockType { get; private set; }

        // 現在ブロック
        private int[,] _block;
        private Point _location;

        public Block(int[,] block, BlockType bt)
        {
            BlockType = bt;
            _block = block;
            _location = new Point(3, 0);
        }

        public void ResetLocation()
        {
            _location = new Point(3, 0);
        }

        public void MoveLocation(int x,int y)
        {
            _location = new Point(_location.X + x, _location.Y + y);
        }

        public void Rotate(int rad)
        {
            var a = rad;
            _block = new int[_block.GetLength(0), _block.GetLength(1)];
        }

        public List<Point> GetBlockPoints()
        {
            var points = new List<Point>();
            for (var y = 0; y < BlockHeight; y++)
            {
                for (int x = 0; x < BlockWidth; x++)
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
            for (int x = 0; x < BlockWidth; x++)
            {
                for (var y = BlockHeight - 1; y >= 0; y--)
                {
                    if (_block[x, y] == 1)
                    {
                        points.Add(new Point(x + _location.X, y + _location.Y));
                        continue;
                    }
                }
            }

            return points;
        }

        public Point GetLocation_BlockTop()
        {
            for (var y = 0; y < SystemProperty.BlockHeight; y++)
            {
                for (int x = 0; x < SystemProperty.BlockWidth; x++)
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
            for (var y = BlockHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < BlockWidth; x++)
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
            for (int x = 0; x < SystemProperty.BlockWidth; x++)
            {
                for (var y = 0; y < SystemProperty.BlockHeight; y++)
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
            for (int x = SystemProperty.BlockWidth - 1; x >= 0; x--)
            {
                for (var y = 0; y < SystemProperty.BlockHeight; y++)
                {
                    if (_block[x, y] == 1)
                    {
                        return new Point(x + _location.X, y + _location.Y);
                    }
                }
            }

            return new Point(-1, -1);
        }
    }
}
