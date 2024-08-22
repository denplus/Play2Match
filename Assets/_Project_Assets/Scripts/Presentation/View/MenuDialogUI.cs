using Scripts.Data;
using Scripts.Data.Signals;
using Scripts.Services.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class MenuDialogUI : BaseUI
    {
        [SerializeField] private Button closeBtn;
        [SerializeField] private Toggle sound;

        private SignalBus _signalBus;
        private IPersistentService _persistentService;

        [Inject]
        private void Init(SignalBus signalBus, IPersistentService persistentService)
        {
            _signalBus = signalBus;
            _persistentService = persistentService;
        }

        private void Start()
        {
            closeBtn.onClick.AddListener(CloseSettingsDialog);
            _signalBus.Subscribe<OpenSettingsSignal>(OpenSettings);
            sound.onValueChanged.AddListener(SoundToggle);
        }

        private void SoundToggle(bool isOn) => 
            _persistentService.Save(new SettingsDto(isOn));

        private void CloseSettingsDialog()
        {
            _signalBus.TryFire(new OpenSettingsSignal(false));
            Hide();
        }

        private void OpenSettings()
        {
            SettingsDto settingsDto = _persistentService.Load<SettingsDto>();
            sound.isOn = settingsDto.IsSoundOn;
            Show();
        }

        private void OnDestroy()
        {
            closeBtn.onClick.RemoveListener(CloseSettingsDialog);
            sound.onValueChanged.RemoveListener(SoundToggle);
            _signalBus.TryUnsubscribe<OpenSettingsSignal>(OpenSettings);
        }
    }
}