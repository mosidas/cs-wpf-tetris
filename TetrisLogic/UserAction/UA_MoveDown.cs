namespace TetrisLogic.UserAction
{
    public class UA_MoveDown : IUserAction
    {
        public void Action(ref Field field, ref Block currentBlock, ref Block holdBlock)
        {
            currentBlock.MoveLocation(0, 1);
        }

        public bool CanAction(Field field, Block block, Block holdBlock)
        {
            foreach(var p in block.GetBlockBottomPoints())
            {
                if (field.GetFieldType(p.X, p.Y + 1) != FieldTypes.empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
