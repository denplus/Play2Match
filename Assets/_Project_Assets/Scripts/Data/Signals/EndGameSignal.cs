namespace Scripts.Data.Signals
{
    public class EndGameSignal
    {
        public int FinalScore;
        public bool TimeFinish;

        public EndGameSignal(int finalScore, bool timeFinish)
        {
            FinalScore = finalScore;
            TimeFinish = timeFinish;
        }
    }
}