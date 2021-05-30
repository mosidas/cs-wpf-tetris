using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TetrisLogic.SystemProperty;

namespace TetrisLogic.UserAction
{

    public class UserActionFactory
    {
        public IUserAction CreateUserAction(ActionType actType)
        {
            switch (actType)
            {
                case ActionType.moveLeft:
                    return new UA_MoveLeft();
                case ActionType.moveRight:
                    return new UA_MoveRight();
                case ActionType.moveDown:
                    return new UA_MoveDown();
                case ActionType.rotateLeft:
                    return new UA_RotateLeft();
                case ActionType.rotateRight:
                    return new UA_RotateRight();
                case ActionType.hold:
                    return new UA_Hold();
                case ActionType.hardDrop:
                    return new UA_HardDrop();
                default:
                    return null;
            }
        }
    }
}
