namespace TetrisLogic.UserAction
{
    public class UserActionFactory
    {
        public IUserAction CreateUserAction(ActionTypes actType)
        {
            switch (actType)
            {
                case ActionTypes.moveLeft:
                    return new UA_MoveLeft();
                case ActionTypes.moveRight:
                    return new UA_MoveRight();
                case ActionTypes.moveDown:
                    return new UA_MoveDown();
                case ActionTypes.rotateLeft:
                    return new UA_RotateLeft();
                case ActionTypes.rotateRight:
                    return new UA_RotateRight();
                case ActionTypes.hold:
                    return new UA_Hold();
                case ActionTypes.hardDrop:
                    return new UA_HardDrop();
                default:
                    return null;
            }
        }
    }
}
