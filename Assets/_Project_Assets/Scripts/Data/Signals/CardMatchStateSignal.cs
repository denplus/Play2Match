namespace Scripts.Data.Signals
{
    public class CardMatchStateSignal
    {
        public bool IsMatched;

        public CardMatchStateSignal(bool isMatched)
        {
            IsMatched = isMatched;
        }
    }
}