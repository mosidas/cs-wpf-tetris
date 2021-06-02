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
        public bool CanAction(Field field, Block block, Block holdBlock);
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock);
    }
}
