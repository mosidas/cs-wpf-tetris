namespace TetrisLogic
{
    public static class SystemProperty
    {
        // FPS
        public static readonly int FPS = 60;

        // フィールド状態
        public static readonly int Empty = 0;
        public static readonly int Block = 1;
        public static readonly int FixedBlock = 2;
        public static readonly int OutOfField = -1;

        public enum ActionType
        {
            nothing = 0,
            moveLeft = 1,
            moveRight = 2,
            moveDown = 3,
            rotateLeft = 4,
            rotateRight = 5,
            hold = 6,
            hardDrop = 7
        }

        public enum BlockType
        {
            nothing,
            T,
            I,
            J,
            L,
            S,
            Z,
            O,
        }

    }
}
