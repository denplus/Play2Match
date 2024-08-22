namespace Scripts.Data.Signals
{
    public class CardMatchStateSignal
    {
        public readonly bool IsMatched;

        public CardMatchStateSignal(bool isMatched)
        {
            IsMatched = isMatched;
        }
    }
}