using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_MoveRight : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(1, 0);
        }

        public bool CanAction(Field field, Block block)
        {
            foreach (var p in block.GetBlockRightPoints())
            {
                if (field.GetFieldState(p.X + 1, p.Y) != Field.FieldState.empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
