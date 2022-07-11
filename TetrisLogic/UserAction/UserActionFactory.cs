namespace TetrisLogic.UserAction
{
    public class UserActionFactory
    {
        public IUserAction CreateUserAction(ActionTypes actType)
        {
            return actType switch
            {
                ActionTypes.moveLeft => new UA_MoveLeft(),
                ActionTypes.moveRight => new UA_MoveRight(),
                ActionTypes.moveDown => new UA_MoveDown(),
                ActionTypes.rotateLeft => new UA_RotateLeft(),
                ActionTypes.rotateRight => new UA_RotateRight(),
                ActionTypes.hold => new UA_Hold(),
                ActionTypes.hardDrop => new UA_HardDrop(),
                _ => new UA_Nothing(),
            };
        }
    }
}
