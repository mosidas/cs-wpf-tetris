using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic
{
    public class Field
    {
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public enum FieldState
        {
            empty,
            block,
            fixedBlock,
            outOfField,
        }

        private static readonly int _width = 10;
        private static readonly int _height = 20;
        private FieldState[,] _fieldState = new FieldState[_width, _height];
        private SystemProperty.BlockType[,] _fieldTypeState = new SystemProperty.BlockType[_width, _height];

        public void InitFieldState()
        {
            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    _fieldState[w, h] = FieldState.empty;
                    _fieldTypeState[w, h] = SystemProperty.BlockType.nothing;
                }
            }
        }

        public List<Point> GetFixedBlockPoints()
        {
            var points = new List<Point>();

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if(_fieldState[w, h] == FieldState.fixedBlock)
                    {
                        points.Add(new Point(w,h));
                    }
                }
            }

            return points;
        }

        public List<SystemProperty.BlockType> GetFixedBlockTypes()
        {
            var types = new List<SystemProperty.BlockType>();

            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    if (_fieldState[w, h] == FieldState.fixedBlock)
                    {
                        types.Add(_fieldTypeState[w, h]);
                    }
                }
            }

            return types;
        }

        public FieldState GetFieldState(int w, int h)
        {
            if(_width <= w || w < 0 || _height <= h || h < 0)
            {
                return FieldState.outOfField;
            }

            return _fieldState[w, h];
        }

        public bool CanSpawn(Block cb)
        {
            foreach(var p in cb.GetBlockPoints())
            {
                if(GetFieldState(p.X, p.Y) != FieldState.empty)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateFieldState(Block cb, bool fixedBlock)
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
                    if (_fieldState[x, y] == FieldState.block)
                    {
                        _fieldState[x, y] = FieldState.empty;
                    }
                }
            }

            if (isFixedBlock)
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldState(point.X, point.Y) != FieldState.outOfField)
                    {
                        _fieldState[point.X, point.Y] = FieldState.fixedBlock;
                        _fieldTypeState[point.X, point.Y] = cb.BlockType;
                    }
                }
            }
            else
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldState(point.X, point.Y) != FieldState.outOfField)
                    {
                        _fieldState[point.X, point.Y] = FieldState.block;
                    }
                }
            }
        }
    }
}
