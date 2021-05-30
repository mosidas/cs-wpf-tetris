using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic
{
    public class BlocksPoolManager
    {
        private List<int[,]> BlocksPool { get; set; }
        private List<SystemProperty.BlockType> BlockTypesPool { get; set; }

        public BlocksPoolManager()
        {
            BlocksPool = new List<int[,]>();
            BlockTypesPool = new List<SystemProperty.BlockType>();
            Reset();
        }

        private void CreateBlocksPool()
        {
            if (BlocksPool.Count > 3)
            {
                return;
            }

            //□□□□
            //□■■□ 
            //□■■□ 
            //□□□□ 
            var blockO = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockO[1, 1] = 1;
            blockO[1, 2] = 1;
            blockO[2, 1] = 1;
            blockO[2, 2] = 1;

            //□■□□
            //□■□□
            //□■□□
            //□■□□
            var blockI = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockI[1, 0] = 1;
            blockI[1, 1] = 1;
            blockI[1, 2] = 1;
            blockI[1, 3] = 1;

            //□□□□
            //□■□□
            //□■■□
            //□■□□
            var blockT = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockT[1, 1] = 1;
            blockT[1, 2] = 1;
            blockT[2, 2] = 1;
            blockT[1, 3] = 1;

            //□□□□ 
            //□■■□ 
            //□■□□ 
            //□■□□ 
            var blockJ = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockJ[1, 1] = 1;
            blockJ[2, 1] = 1;
            blockJ[1, 2] = 1;
            blockJ[1, 3] = 1;

            //□□□□ 
            //□■■□ 
            //□□■□ 
            //□□■□ 
            var blockL = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockL[1, 1] = 1;
            blockL[2, 1] = 1;
            blockL[2, 2] = 1;
            blockL[2, 3] = 1;

            //□□□□ 
            //□□■□ 
            //□■■□ 
            //□■□□ 
            var blockZ = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockZ[1, 2] = 1;
            blockZ[2, 1] = 1;
            blockZ[2, 2] = 1;
            blockZ[1, 3] = 1;

            //□□□□ 
            //□■□□ 
            //□■■□ 
            //□□■□ 
            var blockS = new int[SystemProperty.BlockWidth, SystemProperty.BlockHeight];
            blockS[1, 1] = 1;
            blockS[2, 1] = 1;
            blockS[2, 2] = 1;
            blockS[2, 3] = 1;

            var list = new List<SystemProperty.BlockType>();

            list.Add(SystemProperty.BlockType.O);
            list.Add(SystemProperty.BlockType.I);
            list.Add(SystemProperty.BlockType.T);
            list.Add(SystemProperty.BlockType.J);
            list.Add(SystemProperty.BlockType.L);
            list.Add(SystemProperty.BlockType.Z);
            list.Add(SystemProperty.BlockType.S);

            list = list.OrderBy(a => Guid.NewGuid()).ToList();

            foreach(var type in list)
            {
                if(type == SystemProperty.BlockType.O)
                {
                    BlocksPool.Add(blockO);
                }
                else if (type == SystemProperty.BlockType.I)
                {
                    BlocksPool.Add(blockI);
                }
                else if (type == SystemProperty.BlockType.T)
                {
                    BlocksPool.Add(blockT);
                }
                else if (type == SystemProperty.BlockType.J)
                {
                    BlocksPool.Add(blockJ);
                }
                else if (type == SystemProperty.BlockType.L)
                {
                    BlocksPool.Add(blockL);
                }
                else if (type == SystemProperty.BlockType.Z)
                {
                    BlocksPool.Add(blockZ);
                }
                else if (type == SystemProperty.BlockType.S)
                {
                    BlocksPool.Add(blockS);
                }

                BlockTypesPool.Add(type);
            }
        }

        public void Reset()
        {
            BlocksPool = new List<int[,]>();
        }

        public Block GetNextBlock()
        {
            CreateBlocksPool();
            var tmp = BlocksPool[0];
            var tmp2 = BlockTypesPool[0];
            var nextBlock = new Block(tmp, tmp2);
            BlocksPool.RemoveAt(0);
            BlockTypesPool.RemoveAt(0);
            return nextBlock;
        }
    }
}
