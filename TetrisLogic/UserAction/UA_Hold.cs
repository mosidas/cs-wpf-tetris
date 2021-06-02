using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_Hold : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            if(holdBlock.BlockType == BlockTypes.nothing)
            {
                holdBlock = new Block(currentBlock.BlockType, true);
                currentBlock = new Block(BlockTypes.nothing);
            }
            else
            {
                var tmp = holdBlock;
                holdBlock = new Block(currentBlock.BlockType, false);
                currentBlock = new Block(tmp.BlockType);
            }
        }

        public bool CanAction(Field field, Block block, Block holdBlock)
        {
            if(holdBlock.BlockType == BlockTypes.nothing)
            {
                return true;
            }
            else
            {
                return holdBlock.CanSwap;
            }
            
        }
    }
}
