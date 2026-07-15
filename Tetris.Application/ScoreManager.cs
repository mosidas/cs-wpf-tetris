using Tetris.Domain;

namespace Tetris.Application
{
    /// <summary>
    /// 実行時のスコア累積。計算(規則)は Domain の ScoreRule に委譲する。
    /// </summary>
    public sealed class ScoreManager
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
            Score += ScoreRule.Calculate(line, tSpin, ren, btb, allClear);
        }
    }
}
