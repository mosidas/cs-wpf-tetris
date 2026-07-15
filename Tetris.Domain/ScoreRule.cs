namespace Tetris.Domain
{
    /// <summary>
    /// スコア規則(純粋計算)。旧 ScoreManager の係数表を式・数値変更なしで移植する。
    /// 時間・可変状態・フレームワークに依存しない。
    /// </summary>
    public static class ScoreRule
    {
        /// <summary>
        /// 消去ライン・T-Spin・REN・B2B・全消しから加点を計算する。
        /// </summary>
        public static int Calculate(int line, TSpinTypes tSpin, int ren, bool btb, bool allClear)
        {
            var score = 0;
            score += AddBaseScore(line, tSpin);
            score += AddAdditonalScore(ren, btb, allClear);
            return score;
        }

        private static int AddAdditonalScore(int ren, bool btb, bool allClear)
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

        private static int AddBaseScore(int line, TSpinTypes tSpin)
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
