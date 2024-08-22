using System.Collections.Generic;
using System.Linq;
using Scripts.Data;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.Controllers;
using Scripts.Services.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class MainScreenUI : BaseUI
    {
        [SerializeField] private MenuDialogUI menuDialogUI;
        
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingsBtn;

        [SerializeField] private List<Toggle> difficultyLevels;

        [SerializeField] private TMP_Text bestScoreTxt;

        private MainScreenController _controller;
        private SignalBus _signalBus;
        private IPersistentService _persistentService;

        [Inject]
        private void Init(SignalBus signalBus, DifficultyLevelData difficultyLevelData, IPersistentService persistentService)
        {
            _signalBus = signalBus;
            _persistentService = persistentService;
            _controller = new MainScreenController(signalBus, difficultyLevelData);
        }

        private void Start()
        {
            startBtn.onClick.AddListener(StartGame);
            settingsBtn.onClick.AddListener(OpenSettings);

            _signalBus.Subscribe<EndGameSignal>(PlayerEndGame);

            UpdateBestScore();
        }

        private void StartGame()
        {
            Toggle activeToggle = difficultyLevels.FirstOrDefault(x => x.isOn);
            int index = 0;
            foreach (var toggle in difficultyLevels)
            {
                if (toggle == activeToggle)
                    break;
                index++;
            }

            _controller.StartGame(index);

            Hide();
        }
        
        private void OpenSettings()
        {
            _controller.OpenSettings();
        }

        private void PlayerEndGame()
        {
            UpdateBestScore();
            Show();
        }

        private void UpdateBestScore()
        {
            ScoreDto prevScore = _persistentService.Load<ScoreDto>();
            bestScoreTxt.text = $"{Utils.TextConstance.BestScore}: {prevScore.BestScore}";
        }

        private void OnDestroy()
        {
            startBtn.onClick.RemoveListener(StartGame);
            settingsBtn.onClick.RemoveListener(OpenSettings);

            _signalBus.TryUnsubscribe<EndGameSignal>(PlayerEndGame);
        }
    }
}