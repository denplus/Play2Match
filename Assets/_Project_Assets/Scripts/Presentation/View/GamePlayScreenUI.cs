using System;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.Controllers;
using Scripts.Services.Services.Interfaces;
using Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Scripts.Presentation.View
{
    public class GamePlayScreenUI : BaseUI
    {
        [SerializeField] private FlexibleGridLayout flexibleGridLayout;
        [SerializeField] private CardUnitView cardUnitPrefab;
        [SerializeField] private Transform cardHolder;

        private SignalBus _signalBus;
        private GamePlayScreenController _controller;

        [Inject]
        private void Init(SignalBus signalBus, ISpawnService spawnService, CardImagesData cardImagesData)
        {
            _signalBus = signalBus;
            _controller = new GamePlayScreenController(signalBus, spawnService, cardHolder, cardImagesData);
        }

        private void Start()
        {
            _signalBus.Subscribe<StartGameSignal>(OnStartGame);
        }

        private void OnStartGame(StartGameSignal signal)
        {
            flexibleGridLayout.AmountX = signal.GridSize.x;
            flexibleGridLayout.AmountY = signal.GridSize.y;
            
            _controller.SpawnCards(signal.GridSize, cardUnitPrefab);
            Show();
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<StartGameSignal>(OnStartGame);
        }
    }
}