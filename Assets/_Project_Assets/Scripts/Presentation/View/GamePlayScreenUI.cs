using System;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.Controllers;
using Scripts.Services.Services.Interfaces;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class GamePlayScreenUI : BaseUI
    {
        [SerializeField] private Button pauseBtn;
        [SerializeField] private Button backBtn;

        [SerializeField] private FlexibleGridLayout flexibleGridLayout;
        [SerializeField] private CardUnitView cardUnitPrefab;
        [SerializeField] private Transform cardHolder;
        
        [SerializeField] private TMP_Text attemptTxt;
        [SerializeField] private TMP_Text scoreTxt;

        private SignalBus _signalBus;
        private GamePlayScreenController _controller;

        [Inject]
        private void Init(SignalBus signalBus, ISpawnService spawnService, CardImagesData cardImagesData)
        {
            _signalBus = signalBus;
            _controller = new GamePlayScreenController(this, signalBus, spawnService, cardHolder, cardImagesData);
        }

        private void Start()
        {
            pauseBtn.onClick.AddListener(PauseGame);
            backBtn.onClick.AddListener(BackMainMenu);

            _signalBus.Subscribe<StartGameSignal>(OnStartGame);
        }

        private void BackMainMenu()
        {
            _signalBus.TryFire(new PlayerEndGameSignal(0));
            Hide();
        }

        private void PauseGame()
        {
            
        }

        public void IncreaseScore(int attempt) => 
            attemptTxt.text = $"{TextConstance.Attempt}: {attempt}";

        public void IncreaseAttempt(int score) => 
            scoreTxt.text = $"{TextConstance.Score}: {score}";

        private void OnStartGame(StartGameSignal signal)
        {
            flexibleGridLayout.AmountX = signal.GridSize.x;
            flexibleGridLayout.AmountY = signal.GridSize.y;

            _controller.SpawnCards(signal.GridSize, cardUnitPrefab);
            Show();
        }

        private void OnDestroy()
        {
            pauseBtn.onClick.RemoveListener(PauseGame);
            backBtn.onClick.RemoveListener(BackMainMenu);

            _signalBus.TryUnsubscribe<StartGameSignal>(OnStartGame);
        }
    }
}