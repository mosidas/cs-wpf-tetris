using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public class UA_MoveLeft : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(-1, 0);
        }

        public bool CanAction(Field field, Block block)
        {
            foreach (var p in block.GetBlockLeftPoints())
            {
                if (field.GetFieldState(p.X - 1, p.Y) != SystemProperty.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
