namespace Scripts.Data.Signals
{
    public class PlayerEndGameSignal
    {
        public int FinalScore;

        public PlayerEndGameSignal(int finalScore)
        {
            FinalScore = finalScore;
        }
    }
}