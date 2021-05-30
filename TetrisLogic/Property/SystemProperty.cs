namespace TetrisLogic
{
    public static class SystemProperty
    {
        // FPS
        public static readonly int FPS = 60;

        // フィールドサイズ
        public static readonly int FieldWidth = 10;
        public static readonly int FieldHeight = 20;

        // フィールド状態
        public static readonly int Empty = 0;
        public static readonly int Block = 1;
        public static readonly int FixedBlock = 2;
        public static readonly int OutOfField = -1;

        // ブロックサイズ
        public static readonly int BlockWidth = 4;
        public static readonly int BlockHeight = 4;

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
