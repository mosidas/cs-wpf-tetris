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
            throw new NotImplementedException();
        }

        public bool CanAction(Field field, Block block)
        {
            throw new NotImplementedException();
        }
    }
}
