namespace TetrisLogic.UserAction
{
    public class UA_Nothing : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            
        }

        public bool CanAction(Field field, Block block, Block holdBlock)
        {
            return true;
        }
    }
}
