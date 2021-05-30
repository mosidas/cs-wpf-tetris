using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_MoveDown : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(0, 1);
        }

        public bool CanAction(Field field, Block block)
        {
            foreach(var p in block.GetBlockBottomPoints())
            {
                if (field.GetFieldState(p.X, p.Y + 1) != SystemProperty.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
