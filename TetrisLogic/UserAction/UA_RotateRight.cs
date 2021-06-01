using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TetrisLogic.UserAction
{
    public class UA_RotateRight : IUserAction
    {
        private int MoveX = 0;
        private int MoveY = 0;
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(MoveX, MoveY);
            currentBlock.RotateRight();
        }

        public bool CanAction(Field field, Block block)
        {
            if(block.BlockType == BlockTypes.O)
            {
                return true;
            }
            else if (block.BlockType == BlockTypes.I)
            {
                return CanAction_SRS_I(field, block);
            }
            else
            {
                return CanAction_SRS(field, block);
            }

        }

        private bool CanAction_SRS_I(Field field, Block block)
        {
            var dummyblock = GetDummyActionBlock(block);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            GetSRSState1_I(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            //GetSRSState2_I(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            //GetSRSState3_I(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            //GetSRSState4_I(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            return false;
        }

        private void GetSRSState1_I(ref Block dummyblock)
        {
            if (dummyblock.Direction == DirectionTypes.B)
            {
                dummyblock.MoveLocation(-2, 0);
            }
            else if(dummyblock.Direction == DirectionTypes.D)
            {
                dummyblock.MoveLocation(2, 0);
            }
        }

        private bool CanAction_SRS(Field field, Block block)
        {
            var dummyblock = GetDummyActionBlock(block);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            GetSRSState1(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            GetSRSState2(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            GetSRSState3(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            GetSRSState4(ref dummyblock);

            if (!ExistsCollisionPoint(field, dummyblock))
            {
                return true;
            }

            return false;
        }

        private void GetSRSState4(ref Block dummyblock)
        {
            if (dummyblock.Direction == DirectionTypes.A || dummyblock.Direction == DirectionTypes.B || dummyblock.Direction == DirectionTypes.C)
            {
                dummyblock.MoveLocation(-1, 0);
                MoveX += -1;
            }
            else if (dummyblock.Direction == DirectionTypes.D)
            {
                dummyblock.MoveLocation(1, 0);
                MoveX += 1;
            }
        }

        private void GetSRSState3(ref Block dummyblock)
        {
            dummyblock.MoveLocation(-1 * MoveX, -1 * MoveY);

            if (dummyblock.Direction == DirectionTypes.B || dummyblock.Direction == DirectionTypes.D)
            {
                dummyblock.MoveLocation(0, 2);
                MoveX = 0;
                MoveY = 2;
            }
            else if (dummyblock.Direction == DirectionTypes.A || dummyblock.Direction == DirectionTypes.C)
            {
                dummyblock.MoveLocation(0, -2);
                MoveX = 0;
                MoveY = -2;
            }
        }

        private void GetSRSState2(ref Block dummyblock)
        {
            if ( dummyblock.Direction == DirectionTypes.B || dummyblock.Direction == DirectionTypes.D)
            {
                dummyblock.MoveLocation(0, -1);
                MoveY += -1;
            }
            else if (dummyblock.Direction == DirectionTypes.A || dummyblock.Direction == DirectionTypes.C)
            {
                dummyblock.MoveLocation(0, 1);
                MoveY += 1;
            }
        }

        private void GetSRSState1(ref Block dummyblock)
        {
            if (dummyblock.Direction == DirectionTypes.A || dummyblock.Direction == DirectionTypes.B || dummyblock.Direction == DirectionTypes.C)
            {
                dummyblock.MoveLocation(-1, 0);
                MoveX += -1;
            }
            else if(dummyblock.Direction == DirectionTypes.D)
            {
                dummyblock.MoveLocation(1, 0);
                MoveX += 1;
            }
        }

        private bool ExistsCollisionPoint(Field field, Block block)
        {
            return block
                .GetBlockPoints()
                .Exists(p => field.GetFieldType(p.X, p.Y) == FieldTypes.fixedBlock || field.GetFieldType(p.X, p.Y) == FieldTypes.outOfField);
        }

        private Block GetDummyActionBlock(Block block)
        {
            var tmp = new Block(block);
            tmp.RotateRight();
            return tmp;
        }
    }
}
