using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisLogic.UserAction
{
    public enum ActionTypes
    {
        nothing = 0,
        moveLeft,
        moveRight,
        moveDown,
        rotateLeft,
        rotateRight,
        hold,
        hardDrop,
    }

    public interface IUserAction
    {
        public bool CanAction(Field field, Block block);
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock);
    }
}
