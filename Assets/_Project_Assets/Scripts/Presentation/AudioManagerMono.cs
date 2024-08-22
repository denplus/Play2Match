using Scripts.Data;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Scripts.Presentation
{
    public class AudioManagerMono : MonoBehaviour
    {
        private SignalBus _signalBus;
        private SoundCollectionData _soundCollection;
        private AudioSource _audioSource;
        private IPersistentService _persistentService;

        [Inject]
        private void Init(SignalBus signalBus, SoundCollectionData soundCollection, IPersistentService persistentService)
        {
            _signalBus = signalBus;
            _soundCollection = soundCollection;
            _persistentService = persistentService;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _signalBus.Subscribe<EndGameSignal>(EndGame);
            _signalBus.Subscribe<CardMatchStateSignal>(CardState);
            _signalBus.Subscribe<CardFlipSignal>(CardFlip);
        }

        private void CardFlip() =>
            PlaySound(_soundCollection.CardClip);

        private void CardState(CardMatchStateSignal signal) =>
            PlaySound(signal.IsMatched ? _soundCollection.Match : _soundCollection.MisMatch);

        private void EndGame(EndGameSignal signal)
        {
            if (signal.TimeFinish)
                PlaySound(_soundCollection.GameEnd);
        }

        private void PlaySound(AudioClip clip)
        {
            SettingsDto settingsDto = _persistentService.Load<SettingsDto>();
            if (settingsDto.IsSoundOn)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<EndGameSignal>(EndGame);
            _signalBus.TryUnsubscribe<CardMatchStateSignal>(CardState);
            _signalBus.TryUnsubscribe<CardFlipSignal>(CardFlip);
        }
    }
}