using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_RotateLeft : IUserAction
    {
        protected int MoveX = 0;
        protected int MoveY = 0;
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(MoveX, MoveY);
            currentBlock.RotateLeft();
        }

        public bool CanAction(Field field, Block block)
        {
            throw new NotImplementedException();
        }
    }
}
