using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TetrisLogic.UserAction
{
    public class UA_RotateRight : IUserAction
    {
        private int move = 0;
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(move, 0);
            currentBlock.RotateRight();
        }

        public bool CanAction(Field field, Block block)
        {
            switch (block.BlockType)
            {
                case BlockTypes.O:
                    return CanAction_O(field, block);
                case BlockTypes.I:
                    return CanAction_I(field, block);
                case BlockTypes.J:
                    return CanAction_J(field, block);
                case BlockTypes.L:
                    return CanAction_L(field, block);
                case BlockTypes.S:
                    return CanAction_S(field, block);
                case BlockTypes.Z:
                    return CanAction_Z(field, block);
                case BlockTypes.T:
                    return CanAction_T(field, block);
                default:
                    return false;
            }
        }
        private int OutOfFieldCount(Field field, List<Point> rotatePoints)
        {
            var count = rotatePoints.Where(p => field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField).Count();
            return count;
        }

        private bool IsBlockbyTheWall(Field field, Block block)
        {
            var count = OutOfFieldCount(field, block.GetBlockRotatePoints());
            return count > 0;
        }

        private bool CanAction_T(Field field, Block block)
        {
            var tmpblock = new Block(block);
            tmpblock.RotateRight();

            if (IsBlockbyTheWall(field, tmpblock))
            {
                if (block.GetBlockLeftPoints().Count == 3)
                {
                    move = 1;
                }
                else
                {
                    move = -1;
                }
                tmpblock.MoveLocation(move, 0);
            }
            else
            {
                move = 0;
            }

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock)
                {
                    return false;
                }
            }

            return true;
        }



        private bool CanAction_Z(Field field, Block block)
        {
            var tmpblock = new Block(block);
            tmpblock.RotateRight();

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanAction_S(Field field, Block block)
        {
            var tmpblock = new Block(block);
            tmpblock.RotateRight();
            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanAction_L(Field field, Block block)
        {
            var tmpblock = new Block(block);
            tmpblock.RotateRight();

            if (IsBlockbyTheWall(field, tmpblock))
            {
                if (block.GetBlockLeftPoints().Count == 3)
                {
                    move = 1;
                }
                else
                {
                    move = -1;
                }
                tmpblock.MoveLocation(move, 0);
            }
            else
            {
                move = 0;
            }

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanAction_J(Field field, Block block)
        {
            var tmpblock = new Block(block);
            tmpblock.RotateRight();

            if (IsBlockbyTheWall(field, tmpblock))
            {
                if (block.GetBlockLeftPoints().Count == 3)
                {
                    move = 1;

                }
                else if (block.GetBlockRightPoints().Count == 3)
                {
                    move = -1;
                }
                tmpblock.MoveLocation(move, 0);
            }
            else
            {
                move = 0;
            }

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanAction_I(Field field, Block block)
        {
            var tmpblock = new Block(block);
            tmpblock.RotateRight();
            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanAction_O(Field field, Block block)
        {
            return true;
        }
    }
}
