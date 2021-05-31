using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_RotateRight : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
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

        private bool CanAction_T(Field field, Block block)
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
            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
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
            foreach (var p in tmpblock.GetBlockRotatePoints())
            {
                if (field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField)
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
