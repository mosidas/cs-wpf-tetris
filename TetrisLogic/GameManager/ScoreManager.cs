namespace TetrisLogic
{
    public class ScoreManager
    {
        public int Score { get; private set; }


        public ScoreManager()
        {
            Reset();
        }

        public void Reset()
        {
            Score = 0;
        }


        public void Add(int line, TSpinTypes tSpin, int ren, bool btb, bool allClear)
        {
            var score = 0;
            score += AddBaseScore(line, tSpin);
            score += AddAdditonalScore(ren, btb, allClear);
            Score += score;

        }

        private int AddAdditonalScore(int ren, bool btb, bool allClear)
        {
            var ratio = 0;
            ratio += ren switch
            {
                0 => 0,
                1 => 0,
                2 => 1,
                3 => 1,
                4 => 2,
                5 => 2,
                6 => 3,
                7 => 3,
                8 => 4,
                9 => 4,
                10 => 4,
                _ => 5,
            };

            ratio += btb ? 1 : 0;

            ratio += allClear ? 10 : 0;

            return ratio * 100;
        }

        private int AddBaseScore(int line, TSpinTypes tSpin)
        {
            var ratio = 0;
            if (tSpin == TSpinTypes.tSpin)
            {
                ratio += line == 1 ? 3 :
                    line == 2 ? 5 :
                    line == 3 ? 7 : 0;
            }
            else
            {
                ratio += line == 1 ? 1 :
                    line == 2 ? 2 :
                    line == 3 ? 3 :
                    line == 4 ? 5 : 0;
            }

            return ratio * 100;
        }
    }
}
