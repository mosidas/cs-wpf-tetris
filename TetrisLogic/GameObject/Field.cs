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
        public Point SpawnPoint { get { return _spawnPoint; } }

        private static readonly int _width = SystemProperty.FieldWidth;
        private static readonly int _height = SystemProperty.FieldHeight;
        private int[,] _fieldState = new int[_width, _height];
        private SystemProperty.BlockType[,] _fieldTypeState = new SystemProperty.BlockType[_width, _height];
        private static readonly Point _spawnPoint = new Point(3, 0);

        public void InitFieldState()
        {
            for (var w = 0; w < _width; w++)
            {
                for (var h = 0; h < _height; h++)
                {
                    _fieldState[w, h] = SystemProperty.Empty;
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
                    if(_fieldState[w, h] == SystemProperty.FixedBlock)
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
                    if (_fieldState[w, h] == SystemProperty.FixedBlock)
                    {
                        types.Add(_fieldTypeState[w, h]);
                    }
                }
            }

            return types;
        }

        public int GetFieldState(int w, int h)
        {
            if(_width <= w || w < 0 || _height <= h || h < 0)
            {
                return SystemProperty.OutOfField;
            }

            return _fieldState[w, h];
        }

        public bool CanSpawn()
        {
            return true;
        }

        public void UpdateFieldState(Block cb, bool isFixedBlock)
        {
            UpdateCurerntBlock(cb, isFixedBlock);

            UpdateLine();
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
                    if (_fieldState[x, y] == SystemProperty.Block)
                    {
                        _fieldState[x, y] = SystemProperty.Empty;
                    }
                }
            }

            if (isFixedBlock)
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldState(point.X, point.Y) != SystemProperty.OutOfField)
                    {
                        _fieldState[point.X, point.Y] = SystemProperty.FixedBlock;
                        _fieldTypeState[point.X, point.Y] = cb.BlockType;
                    }
                }
            }
            else
            {
                foreach (var point in cb.GetBlockPoints())
                {
                    if (GetFieldState(point.X, point.Y) != SystemProperty.OutOfField)
                    {
                        _fieldState[point.X, point.Y] = SystemProperty.Block;
                    }
                }
            }
        }
    }
}
