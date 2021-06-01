using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_RotateLeft : UA_RotateRight
    {
        public new void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(MoveX, MoveY);
            currentBlock.RotateLeft();
        }

        protected new Block GetDummyActionBlock(Block block)
        {
            var tmp = new Block(block);
            tmp.RotateLeft();
            return tmp;
        }
    }
}
