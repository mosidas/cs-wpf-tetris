using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic
{
    public interface IUserAction
    {
        public bool CanAction(Field field, Block block);
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock);
    }
}
