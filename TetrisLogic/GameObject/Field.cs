using System.Collections.Generic;
using System.Drawing;

namespace TetrisLogic
{
    public enum FieldTypes
    {
        empty,
        block,
        fixedBlock,
        outOfField,
    }

    public class Field
    {
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        private static readonly int _width = 10;
        private static readonly int _height = 20;
        private FieldTypes[,] _fieldState = new FieldTypes[_height, _width];
        private BlockTypes[,] _fieldTypeState = new BlockTypes[_height, _width];

        public void InitField()
        {
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    _fieldState[row, col] = FieldTypes.empty;
                    _fieldTypeState[row, col] = BlockTypes.nothing;
                }
            }
        }

        public List<(Point,BlockTypes)> GetFieldBlockPointAndTypePairs()
        {
            var pairs = new List<(Point, BlockTypes)>();

            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    if (_fieldState[row, col] != FieldTypes.empty)
                    {
                        pairs.Add((new Point(col, row), _fieldTypeState[row, col]));
                    }
                }
            }

            return pairs;
        }

        public List<Point> GetFieldBlockPoints()
        {
            var points = new List<Point>();

            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    if (_fieldState[row, col] != FieldTypes.empty)
                    {
                        points.Add(new Point(col, row));
                    }
                }
            }

            return points;
        }

        public List<Point> GetFixedBlockPoints()
        {
            var points = new List<Point>();

            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    if(_fieldState[row, col] == FieldTypes.fixedBlock)
                    {
                        points.Add(new Point(col, row));
                    }
                }
            }

            return points;
        }

        public FieldTypes GetFieldType(int col, int row)
        {
            if(_width <= col || col < 0 || _height <= row || row < 0)
            {
                return FieldTypes.outOfField;
            }

            return _fieldState[row, col];
        }

        public bool ExistsCollisionPoint(Block block)
        {
            return block
                .GetBlockPoints()
                .Exists(p => GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || GetFieldType(p.X, p.Y) == FieldTypes.outOfField);
        }

        public void UpdateField(Block cb, bool fixedBlock)
        {
            UpdateCurerntBlock(cb, fixedBlock);

            if(fixedBlock)
            {
                UpdateLine();
            }
            
        }

        private void UpdateLine()
        {
            // TODO:
        }

        private void UpdateCurerntBlock(Block cb, bool isFixedBlock)
        {
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    if (_fieldState[row, col] == FieldTypes.block)
                    {
                        _fieldState[row, col] = FieldTypes.empty;
                    }
                }
            }

            if (isFixedBlock)
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldType(point.X, point.Y) != FieldTypes.outOfField)
                    {
                        _fieldState[point.Y, point.X] = FieldTypes.fixedBlock;
                        _fieldTypeState[point.Y, point.X] = cb.BlockType;
                    }
                }
            }
            else
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldType(point.X, point.Y) != FieldTypes.outOfField)
                    {
                        _fieldState[point.Y, point.X] = FieldTypes.block;
                        _fieldTypeState[point.Y, point.X] = cb.BlockType;
                    }
                }
            }
        }
    }
}
