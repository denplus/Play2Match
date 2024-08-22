namespace Scripts.Data.Signals
{
    public class OpenSettingsSignal
    {
        public bool IsOpen;

        public OpenSettingsSignal(bool isOpen)
        {
            IsOpen = isOpen;
        }
    }
}