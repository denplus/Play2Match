namespace Scripts.Data.Signals
{
    public class EndGameSignal
    {
        public int FinalScore;
        public readonly bool TimeFinish;

        public EndGameSignal(int finalScore, bool timeFinish)
        {
            FinalScore = finalScore;
            TimeFinish = timeFinish;
        }
    }
}