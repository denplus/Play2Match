namespace Scripts.Data
{
    public class SettingsDto
    {
        public readonly bool IsSoundOn;

        public SettingsDto(bool isSoundOn)
        {
            IsSoundOn = isSoundOn;
        }
    }
}