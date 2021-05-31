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
        private FieldTypes[,] _fieldState = new FieldTypes[_width, _height];
        private BlockTypes[,] _fieldTypeState = new BlockTypes[_width, _height];

        public void InitField()
        {
            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    _fieldState[w, h] = FieldTypes.empty;
                    _fieldTypeState[w, h] = BlockTypes.nothing;
                }
            }
        }

        public List<(Point,BlockTypes)> GetFieldBlockPointAndTypePairs()
        {
            var ret = new List<(Point, BlockTypes)>();

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if (_fieldState[w, h] != FieldTypes.empty)
                    {
                        ret.Add((new Point(w, h), _fieldTypeState[w, h]));
                    }
                }
            }

            return ret;
        }

        public List<Point> GetFieldBlockPoints()
        {
            var ret = new List<Point>();

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if (_fieldState[w, h] != FieldTypes.empty)
                    {
                        ret.Add(new Point(w, h));
                    }
                }
            }

            return ret;
        }

        public List<Point> GetFixedBlockPoints()
        {
            var points = new List<Point>();

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if(_fieldState[w, h] == FieldTypes.fixedBlock)
                    {
                        points.Add(new Point(w,h));
                    }
                }
            }

            return points;
        }

        public List<BlockTypes> GetFixedBlockTypes()
        {
            var types = new List<BlockTypes>();

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if (_fieldState[w, h] == FieldTypes.fixedBlock)
                    {
                        types.Add(_fieldTypeState[w, h]);
                    }
                }
            }

            return types;
        }

        public FieldTypes GetFieldType(int w, int h)
        {
            if(_width <= w || w < 0 || _height <= h || h < 0)
            {
                return FieldTypes.outOfField;
            }

            return _fieldState[w, h];
        }

        public bool CanSpawn(Block cb)
        {
            foreach(var p in cb.GetBlockPoints())
            {
                if(GetFieldType(p.X, p.Y) != FieldTypes.empty)
                {
                    return false;
                }
            }
            return true;
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
            for (int x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    if (_fieldState[x, y] == FieldTypes.block)
                    {
                        _fieldState[x, y] = FieldTypes.empty;
                    }
                }
            }

            if (isFixedBlock)
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldType(point.X, point.Y) != FieldTypes.outOfField)
                    {
                        _fieldState[point.X, point.Y] = FieldTypes.fixedBlock;
                        _fieldTypeState[point.X, point.Y] = cb.BlockType;
                    }
                }
            }
            else
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldType(point.X, point.Y) != FieldTypes.outOfField)
                    {
                        _fieldState[point.X, point.Y] = FieldTypes.block;
                        _fieldTypeState[point.X, point.Y] = cb.BlockType;
                    }
                }
            }
        }
    }
}
