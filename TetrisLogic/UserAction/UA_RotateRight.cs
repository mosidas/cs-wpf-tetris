using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TetrisLogic.UserAction
{
    public class UA_RotateRight : IUserAction
    {
        protected int MoveX = 0;
        protected int MoveY = 0;
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(MoveX, MoveY);
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

        protected int OutOfFieldCount(Field field, List<Point> rotatePoints)
        {
            var count = rotatePoints.Where(p => field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField).Count();
            return count;
        }

        protected int WhichWallNearBy(Field field, Block block)
        {
            var count = OutOfFieldCount(field, block.GetBlockRotatePoints());
            if (count == 0)
            {
                return 0;
            }
            return 1;
        }

        protected Block GetDummyActionBlock(Block block)
        {
            var tmp = new Block(block);
            tmp.RotateRight();
            return tmp;
        }

        protected bool CanAction_T(Field field, Block block)
        {
            var tmpblock = GetDummyActionBlock(block);

            switch (WhichWallNearBy(field, tmpblock))
            {
                case 1:
                    MoveX = 1;
                    MoveY = 0;
                    break;
                case 2:
                    MoveX = -1;
                    MoveY = 0;
                    break;
                case 3:
                    MoveX = 0;
                    MoveY = -1;
                    break;
                default:
                    MoveX = 0;
                    MoveY = 0;
                    break;
            }

            tmpblock.MoveLocation(MoveX, MoveY);

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool CanAction_Z(Field field, Block block)
        {
            var tmpblock = GetDummyActionBlock(block);

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool CanAction_S(Field field, Block block)
        {
            var tmpblock = GetDummyActionBlock(block);

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool CanAction_L(Field field, Block block)
        {
            var tmpblock = GetDummyActionBlock(block);

            switch (WhichWallNearBy(field, tmpblock))
            {
                case 1:
                    MoveX = 1;
                    MoveY = 0;
                    break;
                case 2:
                    MoveX = -1;
                    MoveY = 0;
                    break;
                case 3:
                    MoveX = 0;
                    MoveY = -1;
                    break;
                default:
                    MoveX = 0;
                    MoveY = 0;
                    break;
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

        protected bool CanAction_J(Field field, Block block)
        {
            var tmpblock = GetDummyActionBlock(block);

            switch (WhichWallNearBy(field, tmpblock))
            {
                case 1:
                    MoveX = 1;
                    MoveY = 0;
                    break;
                case 2:
                    MoveX = -1;
                    MoveY = 0;
                    break;
                case 3:
                    MoveX = 0;
                    MoveY = -1;
                    break;
                default:
                    MoveX = 0;
                    MoveY = 0;
                    break;
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

        protected bool CanAction_I(Field field, Block block)
        {
            var tmpblock = GetDummyActionBlock(block);

            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool CanAction_O(Field field, Block block)
        {
            return true;
        }
    }
}
