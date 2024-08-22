using System.Collections.Generic;
using System.Linq;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class MainScreenUI : BaseUI
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingsBtn;

        [SerializeField] private List<Toggle> difficultyLevels;

        [SerializeField] private TMP_Text bestScoreTxt;

        private MainScreenController _controller;
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus, DifficultyLevelData difficultyLevelData)
        {
            _signalBus = signalBus;
            _controller = new MainScreenController(signalBus, difficultyLevelData);
        }

        private void Start()
        {
            startBtn.onClick.AddListener(StartGame);
            startBtn.onClick.AddListener(OpenSettings);

            _signalBus.Subscribe<PlayerEndGameSignal>(PlayerEndGame);
        }

        private void OpenSettings() =>
            _controller.OpenSettings();

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

        private void PlayerEndGame() =>
            Show();

        private void OnDestroy()
        {
            startBtn.onClick.RemoveListener(StartGame);
            startBtn.onClick.RemoveListener(OpenSettings);

            _signalBus.TryUnsubscribe<PlayerEndGameSignal>(PlayerEndGame);
        }
    }
}